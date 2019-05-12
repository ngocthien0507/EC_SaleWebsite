using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Areas.Admin.Controllers
{
    public class RevenueController : Controller
    {
        // GET: Admin/Revenue
        public ActionResult Index(string searchString, DateTime? searchDate, long? searchID, int page = 1, int pageSize = 10)
        {
            var dao = new BillAdDAO();
            var model = dao.ListAllPaging(searchString, searchDate, searchID, page, pageSize);
            ViewBag.SearchString = searchString;
            ViewBag.SearchDate = searchDate;
            ViewBag.SearchID = searchID;
            return View(model);
        }
        public ActionResult Revenue(int? month,int? year, string searchString, int page = 1, int pageSize = 3)
        {
            var dao = new BillAdDAO();
            if (month == null)
            {
                month = DateTime.Now.Month;
            }
            if (year == null)
            {
                year = DateTime.Now.Year;
            }
            var model = dao.ListAll(searchString, page, pageSize);
            var totalMoneyThisMonth = dao.getMoneyByMonth(month, year);
            var totalMoneyThisYear = dao.getMoneyByYear(year);
            var totalmoneyMerchant = dao.getMoneyByMerchant(searchString);
            var totalmoney = dao.GetTotalMoney();
            ViewBag.RevenueYear = totalMoneyThisYear;
            ViewBag.RevenueMonth = totalMoneyThisMonth;
            ViewBag.RevenueTotal = totalmoney;
            ViewBag.RevenueMerchant = totalmoneyMerchant;
            ViewBag.Month = month;
            ViewBag.Year = year;
            ViewBag.SearchString = searchString;
            return View(model);
        }
        
    }
}