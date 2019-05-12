using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMDT.Areas.Admin.Controllers
{
    public class ProductsController : BaseController
    {
        // GET: Admin/Products
        public ActionResult Index(string searchName,long? searchCate, int page = 1, int pageSize = 10)
        {
            var dao = new ProductDAO();
            var model = dao.ListAllPaging(searchName, searchCate, page, pageSize);
            ViewBag.ListProductCategory = new ProductCategoryDAO().GetAll();
            ViewBag.SearchName = searchName;
            ViewBag.SearchCate = searchCate;
            return View(model);
        }
        public void BlockProduct(long id)
        {
            new ProductDAO().BlockProduct(id);
            setAlert("Đã hủy sản phẩm", "success");
        }
        public void AcceptProduct(long id)
        {
            new ProductDAO().AcceptProduct(id);
            setAlert("Đã duyệt sản phẩm", "success");
        }
        public ActionResult OrderManager(long? searchCode,string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new TotalOrderDAO();
            var model = dao.ListAllPaging(searchCode, searchString, page, pageSize);
            ViewBag.SearchCode = searchCode;
            return View(model);
        }
        public ActionResult OrderDetail(long id)
        {
            return View();
        }
    }
}