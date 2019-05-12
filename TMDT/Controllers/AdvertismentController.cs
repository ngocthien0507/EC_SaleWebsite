using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Controllers
{
    public class AdvertismentController : Controller
    {
        // GET: Advertisment
        public ActionResult Index()
        {
            var user = (TMDT.Common.UserLogin)Session[Common.CommonConstants.USER_SESSION];
            var model = new OrderAdvertisementDAO().GetByIdShop(user.UserID);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ListIfnoLocationAD = new LocationAdDAO().GetAll();
            setViewBagForCreate();
            return View();
        }
        [HttpPost]
        public ActionResult Create(advertisement adver)
        {
            if (ModelState.IsValid)
            {
                var merchant = (Account)Session[Common.CommonConstants.USER_INFO_SESSION];
                var daoAd = new advertisementDAO();
                var daoOrderAd = new OrderAdvertisementDAO();
                adver.Merchant = merchant.ID;
                adver.CTR = 0;
                adver.Status = false;
                var price = new LocationAdDAO().GetDetail(adver.Location).Price;
                long id = daoAd.Insert(adver);
                if (id > 0)
                {
                    var orderAd = new OrderAdvertisement();
                    orderAd.IDAd = id;
                    orderAd.IDmerchant = adver.Merchant;
                    orderAd.StartDate = adver.ActiveDate;
                    orderAd.EndDate = adver.EndDate;
                    TimeSpan Days = (TimeSpan)(orderAd.EndDate - orderAd.StartDate);
                    orderAd.Price = Days.Days * price;
                    orderAd.Status = 0;
                    orderAd.CreateDate = DateTime.Now;
                    long idorder = daoOrderAd.Insert(orderAd);
                    if (idorder > 0)
                    {
                        return RedirectToAction("Index", "Advertisment");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Thêm sản phẩm thất bại");
                }
            }
            return View("Index");
        }

        public void setViewBagForCreate(long? id = null)
        {
            var dao = new LocationAdDAO();
            ViewBag.ListLocation = new SelectList(dao.GetAll(), "ID", "Name", id);
        }
        public JsonResult FindTotalMoney(DateTime? EndDate, DateTime? StartDate, string IdLocation)
        {
            TimeSpan? Days = EndDate - StartDate;
            TimeSpan Songay;
            if (Days == null)
            {
                Songay = TimeSpan.Zero;
            }
            else
            {
                 Songay = (TimeSpan)Days;
            }
            var location = new LocationAdDAO().GetDetail(IdLocation);
            decimal? totalmoney = null;
            var allDate = new OrderAdvertisementDAO().GetAllSiteAD();
            if (IdLocation == "SLIDE")
            {

                totalmoney = Songay.Days * location.Price;
                return Json(new
                {
                    val = totalmoney.GetValueOrDefault(0).ToString("N0"),
                    status = true
                });
            }
            else
            {
                foreach (var item in allDate)
                {
                    if (item.advertisement.Location == IdLocation)
                    {
                        if ((StartDate >= item.StartDate && StartDate <= item.EndDate) || (EndDate >= item.StartDate && EndDate <= item.EndDate) || (StartDate <= item.StartDate && EndDate >= item.EndDate))
                        {
                            return Json(new
                            {
                                val = "Lịch đã được chọn", // ngày bắt đầu thuộc khoảng có rồi
                                status = false
                            });
                        }
                    }
                }
                totalmoney = Songay.Days * location.Price;
                return Json(new
                {

                    val = totalmoney.GetValueOrDefault(0).ToString("N0"),
                    status = true
                });
            }


        }
        public ActionResult ShowAll()
        {
            var user = (TMDT.Common.UserLogin)Session[Common.CommonConstants.USER_SESSION];
            var model = new advertisementDAO().GetByIdShop(user.UserID);
            return View(model);
        }

        public string UploadPicture(HttpPostedFileBase file)
        {
            //xử lý upload
            file.SaveAs(Server.MapPath("~/Data/Advertisment/" + file.FileName));
            return "/Data/Advertisment/" + file.FileName;
        }
    }
}