using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TMDT
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*botdetect}",
                  new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
            );
            routes.MapRoute(
                name: "Register",
                url: "dang-ky",
                defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
            );
            routes.MapRoute(
                name: "Activating Account",
                url: "kich-hoat-tk",
                defaults: new { controller = "User", action = "ActivatingUser", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
            );
            routes.MapRoute(
                name: "Forgot Password",
                url: "quen-mat-khau",
                defaults: new { controller = "User", action = "ForgotPassword", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
            );
            routes.MapRoute(
                name: "Register Merchant",
                url: "dang-ky-ban-hang",
                defaults: new { controller = "User", action = "RegisterMerchant", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
            );
            routes.MapRoute(
                name: "Check Username",
                url: "check-username",
                defaults: new { controller = "User", action = "CheckUsername", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
            );
            routes.MapRoute(
                name: "About",
                url: "gioi-thieu",
                defaults: new { controller = "About", action = "About", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
            );
            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Contact", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
            );
            routes.MapRoute(
                name: "News",
                url: "tin-tuc/{metatitle}-{id}",
                defaults: new { controller = "Content", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
            );
            routes.MapRoute(
                name: "Content Detail",
                url: "tin-tuc",
                defaults: new { controller = "Content", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Tag",
                url: "tag/{tagId}",
                defaults: new { controller = "Content", action = "Tag", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Product",
                url: "san-pham",
                defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Product Category",
                url: "san-pham/{metatitle}-{cateId}",
                defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Shop's Products",
                url: "san-pham/{shopname}/{id}",
                defaults: new { controller = "Product", action = "ShopProducts", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Product Detail",
                url: "chi-tiet/{metatitle}-{id}",
                defaults: new { controller = "Product", action = "ProductDetail", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Add Cart Item",
                url: "them-vao-gio",
                defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Cart",
                url: "gio-hang",
                defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Payment",
                url: "thanh-toan",
                defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Shop Manager",
                url: "quan-ly-shop",
                defaults: new { controller = "ShopManager", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Merchants Creates Product",
                url: "quan-ly-shop/dang-san-pham",
                defaults: new { controller = "ShopManager", action = "Create", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Shop Manager Order",
                url: "quan-ly-shop/quan-ly-don-hang",
                defaults: new { controller = "ShopManager", action = "OrderManager", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Shop Manager Order Detail",
                url: "quan-ly-shop/quan-ly-don-hang/chi-tiet-don-hang/{idShopOrder}",
                defaults: new { controller = "ShopManager", action = "OrderDetailManager", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Merchant's Order History",
                url: "lich-su-mua-hang/{idcus}",
                defaults: new { controller = "User", action = "OrderHistory", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Merchant's Order History Detail",
                url: "chi-tiet-don-hang/{orderid}",
                defaults: new { controller = "User", action = "OrderHistoryDetail", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Advertisment",
                url: "quang-cao",
                defaults: new { controller = "Advertisment", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Advertisment Creating",
                url: "quang-cao/gui-yeu-cau",
                defaults: new { controller = "Advertisment", action = "Create", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Advertisment ListAll",
                url: "quang-cao/xem-quang-cao",
                defaults: new { controller = "Advertisment", action = "ShowAll", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Account Manager",
                url: "quan-ly-tai-khoan/{id}",
                defaults: new { controller = "User", action = "AccountManager", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
                );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Controllers" }
            );
        }
    }
}
