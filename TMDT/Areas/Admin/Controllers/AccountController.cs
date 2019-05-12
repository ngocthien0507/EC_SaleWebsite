using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TMDT.Common;

namespace TMDT.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Admin/Account
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new AccountDAO();
            var model = dao.ListAllPaging(searchString,page, pageSize);
            // lấy danh sách merchant theo idMerchant trong User
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Account account)
        {
            if (ModelState.IsValid)
            {
                var dao = new AccountDAO();
                var EncryptingMD5Pw = Encryptor.MD5Hash(account.Password);
                account.Password = EncryptingMD5Pw;
                account.CreateDate = DateTime.Now;
                account.Level = 0;
                long id = dao.Insert(account);
                if (id > 0)
                {
                    setAlert("Thêm tài khoản thành công", "success");
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm tài khoản thất bại");
                }
            }
            return View("Index");
        }

        public ActionResult EditUser(long id)
        {
            var user = new AccountDAO().GetInfoByID(id);
            return View(user);
        }
        [HttpPost]
        public ActionResult EditUser(Account account)
        {
            if (ModelState.IsValid)
            {
                var dao = new AccountDAO();
                if (!String.IsNullOrEmpty(account.Password))
                {
                    var EncryptingMD5Pw = Encryptor.MD5Hash(account.Password);
                    account.Password = EncryptingMD5Pw;
                }
                bool update = dao.Update(account);
                if (update)
                {
                    setAlert("Sửa thông tin thành công", "success");
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thông tin thất bại");
                }
            }
            return View("Index");
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
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            Session[CommonConstants.USER_INFO_SESSION] = null;
            return Redirect("/");
        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            new AccountDAO().Delete(id);
            setAlert("Đã xóa tài khoản", "success");
            return RedirectToAction("Index");
        }
    }
}