using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            UpdateToTalOrder();
            new advertisementDAO().SetAdOutOfDate();
            ViewBag.Slides = new advertisementDAO().GetAvailableSlideAD();
            var productDAO = new ProductDAO();
            ViewBag.NewProduct = productDAO.ListNewProduct(4);
            ViewBag.FeatureProduct = productDAO.ListFeatureProduct(4);
            return View();
        }

        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var model = new MenuDAO().ListByGroupID(1);
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult TopMenu()
        {
            var model = new MenuDAO().ListByGroupID(2);
            var loginmodel = new MenuDAO().ListByGroupID(3);
            var loginmerchant = new MenuDAO().ListByGroupID(4);
            var registermerchant = new MenuDAO().ListByGroupID(5);
            ViewBag.LoginMenu = loginmodel;
            ViewBag.LoginMerchant = loginmerchant;
            ViewBag.RegisterMerchant = registermerchant;
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            var model = new FooterDAO().GetFooter();
            return PartialView(model);
        }
        [ChildActionOnly]
        public ActionResult AdLeftRight()
        {
            var model = new advertisementDAO().GetAvailableSiteAD();
            return PartialView(model);
        }
        public void UpdateToTalOrder()
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
    }
}