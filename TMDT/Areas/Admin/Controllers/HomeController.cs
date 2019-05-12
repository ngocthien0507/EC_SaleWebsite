using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Common;

namespace TMDT.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult _Topbar()
        {
            var model = (UserLogin)Session[CommonConstants.USER_SESSION];
            return PartialView(model);
        }
    }
}