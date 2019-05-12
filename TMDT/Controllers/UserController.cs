using BotDetect.Web.Mvc;
using Common;
using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDT.Common;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            var otp = new OTPSimulation().MakeOTP();
            string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/Client/template/ResetPassword.html"));
            content = content.Replace("{{Password}}", otp);
            var EncryptingMD5Pw = Encryptor.MD5Hash(otp);         
            new MailHelper().SendMail(Email, "Yêu cầu Reset Password", content);
            new AccountDAO().ResetPassWord(EncryptingMD5Pw, Email);
            ViewBag.Reset = otp;
            return View();
        }
        [HttpGet]
        public ActionResult ActivatingUser()
        {
            if (Session[CommonConstants.OTP_SESSION] == null)
            {
                if (Session[CommonConstants.USER_INFO_SESSION] != null)
                {
                    var otp = new OTPSimulation().MakeOTP();
                    Session[CommonConstants.OTP_SESSION] = otp;
                    var user = (Account)Session[CommonConstants.USER_INFO_SESSION];
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/Client/template/CodeActive.html"));
                    content = content.Replace("{{Username}}", user.Username);
                    content = content.Replace("{{CodeActive}}", otp);
                    new MailHelper().SendMail(user.Email, "Mã kích hoạt tài khoản ", content);
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult ActivatingUser(string OTP)
        {
            if (ModelState.IsValid)
            {
                var otp = Session[CommonConstants.OTP_SESSION].ToString();
                var user = (Account)Session[CommonConstants.USER_INFO_SESSION];
                if (otp.Equals(OTP))
                {
                    var isActivated = new AccountDAO().UpdateStatusUser(user.Username);
                    Session[CommonConstants.OTP_SESSION] = null;
                    Session[CommonConstants.USER_INFO_SESSION] = null;
                    Session[CommonConstants.ACTIVATED_USER_SUCCESS] = "Kích hoạt thành công";
                    return Redirect("/dang-nhap");
                }
                else
                {
                    ModelState.AddModelError("", "Mã kích hoạt không chính xác");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCaptcha", "Mã xác nhận không chính xác!")]
        public ActionResult Register(RegisterModel account)
        {
            if (ModelState.IsValid)
            {
                var dao = new AccountDAO();
                var EncryptingMD5Pw = Encryptor.MD5Hash(account.Password);
                account.Password = EncryptingMD5Pw;
                var user = new Account();
                user.Username = account.Username;
                user.Password = account.Password;
                user.Name = account.Name;
                user.Phone = account.Phone;
                user.Address = account.Address;
                user.Email = account.Email;
                user.Status = false;
                user.CreateDate = DateTime.Now;
                user.Level = 0;
                var result = dao.Insert(user);
                if (result > 0)
                {
                    var otp = new OTPSimulation().MakeOTP();
                    Session[CommonConstants.OTP_SESSION] = otp;
                    Session.Add(CommonConstants.USER_INFO_SESSION, user);
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/Client/template/CodeActive.html"));
                    content = content.Replace("{{Username}}", user.Username);
                    content = content.Replace("{{CodeActive}}", otp);
                    new MailHelper().SendMail(user.Email, "Mã kích hoạt tài khoản", content);
                    account = new RegisterModel();
                    return Redirect("/kich-hoat-tk");
                }
                else
                {
                    return Redirect("/dang-ky-that-bai");
                }
            }
            return View(account);
        }

        [HttpGet]
        public ActionResult RegisterMerchant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterMerchant(RegisterMerchant merchant)
        {
            if (ModelState.IsValid)
            {
                var user = (Account)Session[CommonConstants.USER_INFO_SESSION];
                var userdao = new AccountDAO();
                user.ShopName = merchant.ShopName;
                user.CMND = merchant.CMND;
                if (merchant.ShopAddress == null)
                {
                    user.ShopAddress = user.Address;
                }
                else
                {
                    user.ShopAddress = merchant.ShopAddress;
                }
                user.ImgCMND = merchant.ImgCMND;
                user.CreateDateShop = DateTime.Now;
                user.StatusShop = false;
                user.Rating = 0;
                var result = new AccountDAO().UpdateMerchant(user);
                if (result == true)
                {
                    new AccountDAO().UpdateLevel(user.Username);
                    merchant = new RegisterMerchant();
                    var userinfo = new AccountDAO().GetInfoByID(user.ID);
                    Session.Add(CommonConstants.USER_INFO_SESSION, userinfo);
                    return Redirect("/");
                }
                else
                {
                    return Redirect("/dang-ky-that-bai");
                }
            }
            return View(merchant);
        }

        public ActionResult NotActivated()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var userdao = new AccountDAO();
                var result = userdao.Login(model.UserName, Encryptor.MD5Hash(model.PassWord));
                if (result == 1)
                {
                    var user = userdao.GetInfoByUsername(model.UserName);
                    if (user.Level == -1)
                    {
                        ModelState.AddModelError("", "Lỗi đăng nhập");
                        return View(model);
                    }
                    Session.Add(CommonConstants.USER_INFO_SESSION, user);
                    var userSession = new UserLogin();
                    if (user.Status == true)
                    {
                        userSession.UserName = model.UserName;
                        userSession.UserID = user.ID;
                        userSession.Level = (int)user.Level;
                        Session.Add(CommonConstants.USER_SESSION, userSession);
                        if (Session["CartSession"] != null)
                        {
                            return Redirect("/thanh-toan");
                        }
                        return Redirect("/");
                    }
                    else
                    {
                        return RedirectToAction("NotActivated");
                    }
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
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
            return View(model);
        }

        public ActionResult AccountManager(long id)
        {
            var account = new AccountDAO().GetInfoByID(id);
            return View(account);
        }
        [HttpPost]
        public ActionResult AccountManager(Account account, string Pass)
        {
            if (ModelState.IsValid)
            {
                var dao = new AccountDAO();
                if (!String.IsNullOrEmpty(Pass))
                {
                    var EncryptingMD5Pw = Encryptor.MD5Hash(Pass);
                    Pass = EncryptingMD5Pw;
                }
                bool update = dao.EditInfo(account, Pass);
                if (update)
                {
                    //setAlert("Sửa thông tin thành công", "success");
                    return RedirectToAction("AccountManager", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thông tin thất bại");
                }
            }
            return View("AccountManager");
        }

        public ActionResult OrderHistory(long idcus)
        {
            var model = new TotalOrderDAO().GetTotalOrderById(idcus);
            return View(model);
        }
        public ActionResult OrderHistoryDetail(long orderid)
        {
            var model = new List<OrderDetail>();
            var totalorder = new TotalOrderDAO().GetDetail(orderid);
            var listshop = new List<ShopOrder>();
            foreach (var item in totalorder.ShopOrders)
            {
                listshop.Add(item);
                foreach (var item1 in item.OrderDetails)
                {
                    model.Add(item1);
                }
            }
            ViewBag.ListShop = listshop;
            return View(model);
        }

        public JsonResult CheckUsername(string username)
        {
            var row = new AccountDAO().FindByUsername(username);
            if (row == null)
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
        public JsonResult FindInfoMerchant(long id)
        {
            var merchant = new AccountDAO().GetInfoByID(id);
            return Json(new
            {
                status = true,
                name = merchant.Name,
                shopname = merchant.ShopName,
                address = merchant.Address,
                phone = merchant.Phone
            });
        }
        public JsonResult CheckEmail(string email)
        {
            var row = new AccountDAO().IsExitsEmail(email);
            if (row == true)
            {
                return Json(new
                {
                    status = false
                });
            }
            else
            {
                return Json(new
                {
                    status = true
                });
            }
        }

        public JsonResult RefuseOrder(long id)
        {
            var totalOrder = new TotalOrderDAO().RefuseOrder(id);

            foreach (var item in totalOrder.ShopOrders)
            {
                new ShopOrderDAO().RefuseOrder(item.ID);
            }
            return Json(new
            {
                status = true
            });
        }

        public JsonResult CheckPhone(string phone)
        {
            var row = new AccountDAO().IsExitsPhone(phone);
            if (row == true)
            {
                return Json(new
                {
                    status = false
                });
            }
            else
            {
                return Json(new
                {
                    status = true
                });
            }
        }

        public JsonResult DeleteSessionUserInfo()
        {
            Session[CommonConstants.USER_SESSION] = null;
            Session[CommonConstants.USER_INFO_SESSION] = null;
            return Json(new
            {
                status = true
            });
        }

        public string UploadPicture(HttpPostedFileBase file)
        {

            //xử lý upload
            file.SaveAs(Server.MapPath("~/Data/CMND/" + file.FileName));
            return "/Data/CMND/" + file.FileName;
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            Session[CommonConstants.USER_INFO_SESSION] = null;
            return Redirect("/");
        }
    }
}