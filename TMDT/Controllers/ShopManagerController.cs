using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Common;

namespace TMDT.Controllers
{
    public class ShopManagerController : Controller
    {
        // GET: ShopManager
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var user = (TMDT.Common.UserLogin)Session[Common.CommonConstants.USER_SESSION];
            var model = new ProductDAO().ListByIdShop(user.UserID, searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        public ActionResult OrderManager(long? searchCode, int page = 1, int pageSize = 10)
        {
            UpdateTotalOrder();
            var user = (TMDT.Common.UserLogin)Session[TMDT.Common.CommonConstants.USER_SESSION];
            var dao = new ShopOrderDAO();
            var model = dao.ListAllPaging(user.UserID, searchCode, page, pageSize);
            ViewBag.SearchCode = searchCode;
            return View(model);
        }

        public ActionResult OrderDetailManager(long idShopOrder)
        {
            var orderDetails = new OrderDetailDAO().ListByIdShopOrder(idShopOrder);
            ViewBag.Customer = new TotalOrderDAO().GetDetail(orderDetails.FirstOrDefault().ShopOrder.IDTotalOrder.Value);
            ViewBag.ShopOrder = new ShopOrderDAO().GetDetail(idShopOrder);
            return View(orderDetails);
        }

        [HttpGet]
        public ActionResult Create()
        {
            setViewBagForEdit();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var rewriteurl = new RewriteURL();
                var merchant = (Account)Session[Common.CommonConstants.USER_INFO_SESSION];
                var dao = new ProductDAO();
                product.MetaTitle = rewriteurl.ConvertToUnSign(product.Name);
                product.CreateBy = merchant.ID;
                product.CreateDate = DateTime.Now;
                product.MetaDescriptions = rewriteurl.ConvertToUnSign(product.Descriptions);
                product.Status = false;
                product.IsHidden = 0;
                long id = dao.Insert(product);
                if (id > 0)
                {
                    //setAlert("Thêm tài khoản thành công", "success");
                    return RedirectToAction("Index", "ShopManager");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm sản phẩm thất bại");
                }
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult EditInfo(long id)
        {
            var model = new ProductDAO().GetDetail(id);
            setViewBagForEdit(model.CategoryID);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditInfo(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDAO();
                var update = dao.Update(product);
                if (update)
                {
                    //setAlert("Sửa thông tin thành công", "success");
                    return RedirectToAction("Index", "ShopManager");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thông tin thất bại");
                }
            }
            return View("Index");
        }
        public void setViewBagForEdit(long? id = null)
        {
            var dao = new ProductCategoryDAO();
            ViewBag.ListProductCategory = new SelectList(dao.GetAll(), "ID", "Name", id);
        }

        public JsonResult HidingProduct(long idsp)
        {
            var hide = new ProductDAO().HidingProduct(idsp);
            if (hide == true)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
        public JsonResult ConfirmOrder(long idShopOrder)
        {
            var confirm = new ShopOrderDAO().ConfirmOrder(idShopOrder);
            if (confirm == true)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
        public JsonResult RatingPoint(long id, int point)
        {
            new RatingDAO().Insert(id, point);
            
            return Json(new
            {
                status = true
            });
        }

        public JsonResult RefuseOrder(long idShopOrder)
        {
            var refuse = new ShopOrderDAO().RefuseOrder(idShopOrder);
            if (refuse == true)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
        public JsonResult CompletedOrder(long idShopOrder)
        {
            var completed = new ShopOrderDAO().CompletedOrder(idShopOrder);
            if (completed == true)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
        public void UpdateTotalOrder()
        {
            var totalorderDAO = new TotalOrderDAO();
            var ListTotalOrder = totalorderDAO.GetAll();
            foreach (var totalOrder in ListTotalOrder)
            {
                int sumcheck = totalOrder.ShopOrders.Count();
                int CountShopOrderStatus0 = 0; // kiểm tra số ShopOrder có status = 0
                int CountShopOrderStatus1 = 0; // kiểm tra số ShopOrder có status = 1
                int CountShopOrderStatus2 = 0; // kiểm tra số ShopOrder có status = 2
                int CountSHopOrderStatus3 = 0; // kiểm tra số ShopOrder có status = 3
                var IdTotal = totalOrder.ID;
                int StatusValue;
                foreach (var shopOrder in totalOrder.ShopOrders)
                {
                    if (shopOrder.Status == 0)
                    {
                        CountShopOrderStatus0 += 1;
                    }
                    else if (shopOrder.Status == 1)
                    {
                        CountShopOrderStatus1 += 1;
                    }
                    else if (shopOrder.Status == 2)
                    {
                        CountShopOrderStatus2 += 1;
                    }
                    else
                    {
                        CountSHopOrderStatus3 += 1;
                    }
                }
                if (CountSHopOrderStatus3 == sumcheck)
                {
                    StatusValue = 3;
                    totalorderDAO.UpdateStatus(IdTotal, StatusValue);
                }
                else if (CountShopOrderStatus2 == sumcheck || (CountShopOrderStatus0 == 0 && CountShopOrderStatus1 == 0))
                {
                    StatusValue = 2;
                    totalorderDAO.UpdateStatus(IdTotal, StatusValue);
                }
                else if (CountShopOrderStatus0 == 0)
                {
                    StatusValue = 1;
                    totalorderDAO.UpdateStatus(IdTotal, StatusValue);
                }
            }
        }
        public string UploadPicture(HttpPostedFileBase file)
        {

            //xử lý upload
            file.SaveAs(Server.MapPath("~/Data/ProductIMG/" + file.FileName));
            return "/Data/ProductIMG/" + file.FileName;
        }
    }
}