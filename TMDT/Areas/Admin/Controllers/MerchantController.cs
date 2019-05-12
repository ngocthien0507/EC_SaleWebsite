using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Areas.Admin.Controllers
{
    public class MerchantController : BaseController
    {
        // GET: Admin/Merchant
        public ActionResult Index(string searchString, int page = 1, int pageSize = 4)
        {
            var dao = new AccountDAO();
            var model = dao.ListAllPagingMerchant(searchString, page, pageSize);
            // lấy danh sách User theo idMerchant trong User
            ViewBag.SearchString = searchString;
            return View(model);
        }

        public ActionResult EditMerchant(long id)
        {
            var user = new AccountDAO().GetInfoByID(id);
            return View(user);
        }
        [HttpPost]
        public ActionResult EditMerchant(Account account)
        {
            if (ModelState.IsValid)
            {
                var dao = new AccountDAO();
                bool update = dao.UpdateMerchant(account);
                if (update)
                {
                    setAlert("Sửa thông tin thành công", "success");
                    return RedirectToAction("Index", "Merchant");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thông tin thất bại");
                }
            }
            return View("Index");
        }

        public ActionResult MerchantDetail(long id)
        {
            var dao = new AccountDAO();
            var model = dao.GetInfoByID(id);
            return View(model);
        }
        public ActionResult BlockMerchant(long id)
        {
            new AccountDAO().BlockMerchant(id);
            setAlert("Đã khóa Shop", "success");
            return RedirectToAction("Index");
        }

        public ActionResult UpdateStatus(long id)
        {
            new AccountDAO().UpdateStatusMerchant(id);
            setAlert("Kích hoạt thành công", "success");
            return RedirectToAction("Index");
        }
    }
}