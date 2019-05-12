using System.Web.Mvc;

namespace TMDT.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", Controller = "Account", id = UrlParameter.Optional },
                namespaces: new[] { "TMDT.Areas.Admin.Controllers" }
            );
        }
    }
}