using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Model.DAO;
using Model.EF;


namespace TMDT.Areas.Admin.Controllers
{
    public class AdvertisementController : BaseController
    {
        // GET: Admin/Advertisement
        public ActionResult Index(string searchName, string searchLocation, int page = 1, int pageSize = 10)
        {
            var dao = new OrderAdvertisementDAO();
            var model = dao.ListAllPaging(searchName, searchLocation, page, pageSize);
            ViewBag.ListLocationAd = new LocationAdDAO().GetAll();
            ViewBag.SearchName = searchName;
            ViewBag.SearchLocation = searchLocation;
            return View(model);
        }
        public void RefuseAd(long id)
        {
            new OrderAdvertisementDAO().RefuseAdvertisement(id);
            setAlert("Đã hủy sản phẩm", "success");
        }
        public void Accepted(long id)
        {
            new OrderAdvertisementDAO().AcceptedAdvertisement(id);
            var info = new OrderAdvertisementDAO().GetDetail(id);
            string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/Admin/template/PaymentRequest.html"));
            content = content.Replace("{{IDORDER}}", info.ID.ToString());
            content = content.Replace("{{Name}}", info.Account.Name);
            content = content.Replace("{{Phone}}", info.Account.Phone);
            content = content.Replace("{{Mail}}", info.Account.Email);
            content = content.Replace("{{Address}}", info.Account.Address);
            content = content.Replace("{{AdName}}", info.advertisement.Content);
            content = content.Replace("{{Link}}", info.advertisement.Link);
            content = content.Replace("{{AdType}}", info.advertisement.LocationAd.Name);
            content = content.Replace("{{StartDate}}", String.Format("{0:dd-MM-yyyy}", info.StartDate));
            content = content.Replace("{{EndDate}}", String.Format("{0:dd-MM-yyyy}", info.EndDate));
            content = content.Replace("{{Money}}", info.Price.GetValueOrDefault(0).ToString("N0"));

            new MailHelper().SendMail(info.Account.Email, "Xác nhận yêu cầu quảng cáo #"+info.ID+" từ HOME SHOPPE", content);

            setAlert("Đã duyệt sản phẩm", "success");
        }
        [HttpGet]
        public ActionResult Payment(long id)
        {
            var model = new OrderAdvertisementDAO().GetDetail(id);
            ViewBag.OrderDetail = model;
            var user = model.Account;
            return View(user);
        }

        [HttpPost]
        public ActionResult Payment(Account account , long IdOrderAD , decimal Money, string TradeCode)
        {
            if (ModelState.IsValid)
            {
                var dao = new BillAdDAO();
                var bill = new BillAd();
                bill.Address = account.Address;
                bill.CMND_ID = account.CMND;
                bill.CreateDate = DateTime.Now;
                bill.IdOrderAd = IdOrderAD;
                bill.Money = Money;
                bill.NameCus = account.Name;
                bill.Phone = account.Phone;
                bill.TradingCode = TradeCode;
                long id = dao.Insert(bill);
                if (id > 0)
                {
                    var IdAds = new OrderAdvertisementDAO().UpdateStatusComplete(IdOrderAD);
                    if (IdAds>0)
                    {
                        new advertisementDAO().ActiveAd(IdAds);
                    }
                    setAlert("Đã thanh toán thành công", "success");
                    return RedirectToAction("Index", "Advertisement");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm tài khoản thất bại");
                }
            }
            return View("Index");
        }

        public ActionResult ListAd(string searchStatus, string searchName, long? searchID, string searchLocation, int page = 1, int pageSize = 10)
        {
            var dao = new advertisementDAO();
            var model = dao.ListAllPaging(searchStatus ,searchName, searchID, searchLocation, page, pageSize);
            ViewBag.ListLocationAd = new LocationAdDAO().GetAll();
            ViewBag.SearchName = searchName;
            ViewBag.SearchLocation = searchLocation;
            ViewBag.SearchStatus = searchStatus;
            return View(model);
        }
        public string uploadPicture(HttpPostedFileBase file)
        {
            file.SaveAs(Server.MapPath("~/Data/Advertisment/" + file.FileName));
            return "/Data/Advertisment/" + file.FileName;
        }

        [HttpGet]
        public ActionResult EditAd(long id)
        {
            var ads = new advertisementDAO().GetDetail(id);
            return View(ads);
        }
        [HttpPost]
        public ActionResult EditAd(advertisement ads)
        {
            if (ModelState.IsValid)
            {
                var dao = new advertisementDAO();
                
                bool update = dao.Update(ads);
                if (update)
                {
                    setAlert("Sửa nội dung quảng cáo thành công", "success");
                    return RedirectToAction("ListAd", "Advertisement");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thông tin thất bại");
                }
            }
            return View("ListAd");
        }
    }

}