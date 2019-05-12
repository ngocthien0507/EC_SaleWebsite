using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Model.EF;

namespace TMDT.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(string searchString,int page = 1, int pageSize = 8)
        {
            int totalRecord = 0;
            var model = new ProductDAO().ListAll(searchString ,page, ref totalRecord, pageSize);
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            int maxPage = 5;
            int totalPage = 0;
            totalPage = (int)Math.Ceiling(((double)totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View(model);
        }
        public ActionResult ShopProducts(long id)
        {
            var model = new ProductDAO().ListProductByIdShop(id);
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDAO().GetAll();
            return PartialView(model);
        }

        public ActionResult Category(long cateId)
        {
            var category = new ProductCategoryDAO().ViewDetail(cateId);
            var ListProductByCategoryID = new ProductDAO().ListByCategoryID(cateId);
            ViewBag.ListProductByCategoryID = ListProductByCategoryID;
            return View(category);
        }
        public ActionResult ProductDetail(long id)
        {
            var model = new ProductDAO().GetDetail(id);
            ViewBag.CategoryDetail = new ProductCategoryDAO().ViewDetail((long)model.CategoryID);
            ViewBag.RelatedProducts = new ProductDAO().ListRelatedProduct((long)model.ID, 3);
            ViewBag.ListProductCategory = new ProductCategoryDAO().GetAll();
            return View(model);
        }
    }
}