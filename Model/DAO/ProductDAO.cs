using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Model.Common;

namespace Model.DAO
{
    public class ProductDAO
    {
        TmdtDbContext db = null;
        public ProductDAO()
        {
            db = new TmdtDbContext();
        }
        /// <summary>
        /// get list all product
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> ListAll(string searchString,int page, ref int totalRecord, int pageSize)
        {
            IQueryable<Product> model = db.Products.Where(x => x.Status == true);
            IEnumerable<Product> a = db.Products.Where(x => x.Status == true);
            if (!string.IsNullOrEmpty(searchString))
            {
                a = a.Where(x => x.Name.Contains(searchString) || x.ProductCategory.Name.Contains(searchString));
                model = model.Where(x => x.Name.Contains(searchString) || x.ProductCategory.Name.Contains(searchString));
            }
            totalRecord = a.Count();
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
        public IEnumerable<Product> ListByIdShop(long id, string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products.Where(x=>x.CreateBy==id);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.ProductCategory.Name.Contains(searchString));
            }
            return model.OrderBy(x => x.IsHidden).ThenBy(x => x.Status).ThenByDescending(x => x.Account1.SaleRate.Rate).ToPagedList(page, pageSize);
        }
        public IEnumerable<Product> ListAllPaging(string searchName,long? searchCate, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchName))
            {
                    model = model.Where(x => x.Name.Contains(searchName) 
                    || x.ProductCategory.Name.Contains(searchName) || x.Account1.Name.Contains(searchName));
            }
            if (searchCate.HasValue)
            {
                model = model.Where(x => x.CategoryID == searchCate);
            }
            return model.OrderBy(x => x.IsHidden).ThenBy(x=>x.Status).ThenByDescending(x=> x.Account1.SaleRate.Rate).ToPagedList(page, pageSize);
        }
        /// <summary>
        /// Get list product by categoryID
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public List<Product> ListByCategoryID(long categoryID)
        {
            return db.Products.Where(x => x.CategoryID == categoryID && x.Status == true).ToList();
        }
        public long Insert(Product entity)
        {
            db.Products.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool Update(Product entity)
        {
            try
            {
                var prod = db.Products.Find(entity.ID);
                prod.Name = entity.Name;
                prod.Descriptions = entity.Descriptions;
                prod.Image = entity.Image;
                prod.CategoryID = entity.CategoryID;
                prod.Detail = entity.Detail;
                prod.Price = entity.Price;
                prod.Quantity = entity.Quantity;
                prod.Status = false;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //có thể làm ghi log
                return false;
            }
        }
        public bool UpdateQuantity(long id,int quantity)
        {
            try
            {
                var product = GetDetail(id);
                product.Quantity -= quantity;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //có thể làm ghi log
                return false;
            }
        }
        public List<Product> GetAll()
        {
            return db.Products.Where(x => x.Status == true).ToList();
        }
        public List<Product> ListProductByIdShop(long id)
        {
            return db.Products.Where(x => x.Status == true && x.CreateBy == id).ToList();
        }
        public List<Product> GetByIDShop(long id)
        {
            return db.Products.Where(x => x.CreateBy == id).ToList();
        }
        public List<Product> ListNewProduct(int top)
        {
            return db.Products.Where(x => x.Status == true).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now && x.Status == true).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }
        public List<Product> ListRelatedProduct(long productID, int top)
        {
            var prod = db.Products.Find(productID);
            return db.Products.Where(x => x.CategoryID == prod.CategoryID && x.ID != prod.ID && x.Status == true).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }
        public Product GetDetail(long id)
        {
            return db.Products.Find(id);
        }
        public bool HidingProduct(long id)
        {
            try
            {
                var prod = GetDetail(id);
                prod.IsHidden = 1;
                prod.Status = false;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool BlockProduct(long id)
        {
            try
            {
                var prod = db.Products.Find(id);
                prod.Status = false;
                prod.IsHidden = 1;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool AcceptProduct(long id)
        {
            try
            {
                var prod = db.Products.Find(id);
                prod.Status = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public string RewriteUrl(string text)
        {
            var rewrite = new RewriteURL();
            var url = rewrite.ConvertToUnSign(text);
            return url;
        }

        public void SetProductOutOfStock()
        {
            var list = GetAll();
            foreach(var item in list)
            {
                if (item.Quantity < 1)
                {
                    item.Status = false;
                }
            }
            db.SaveChanges();
        }
    }
}

