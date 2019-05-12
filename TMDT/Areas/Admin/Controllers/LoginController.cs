using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Areas.Admin.Models;
using TMDT.Common;

namespace TMDT.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new AccountDAO();
                var result = dao.Login(model.Username, Encryptor.MD5Hash(model.Password));
                if(result == 1)
                {
                    var user = dao.GetInfoByUsername(model.Username);
                    var userSession = new UserLogin();
                    userSession.UserName = model.Username;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_INFO_SESSION, user);
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Revenue", "Revenue");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else if(result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không chính xác");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập thật bại");
                }
            }
            return View("Index");
        }
    }
}