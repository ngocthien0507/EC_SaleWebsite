using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.DAO
{
    public class ShopOrderDAO
    {
        TmdtDbContext db = null;
        public ShopOrderDAO()
        {
            db = new TmdtDbContext();
        }
        public long Insert(ShopOrder shoporder)
        {
            db.ShopOrders.Add(shoporder);
            db.SaveChanges();
            return shoporder.ID;
        }
        public IEnumerable<ShopOrder> ListAllPaging(long idMerchant,long? searchCode, int page, int pageSize)
        {
            IQueryable<ShopOrder> model = db.ShopOrders.Where(x=>x.IDMerchant==idMerchant);
            if (searchCode.HasValue)
            {
                model = model.Where(x => x.ID == searchCode);
            }
            return model.OrderByDescending(x => x.Status == 0 || x.Status==1).ThenBy(x=>x.TotalOrder.CreateDate).ToPagedList(page, pageSize);
        }

        public ShopOrder GetDetail(long id)
        {
            return db.ShopOrders.Find(id);
        }
        public bool ConfirmOrder(long id)
        {
            try
            {
                var shoporder = db.ShopOrders.Find(id);
                shoporder.Status = 1;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateTotalPrice(long id, decimal? totalmoney)
        {
            try
            {
                var shoporder = db.ShopOrders.Find(id);
                shoporder.TotalPrice = totalmoney;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool RefuseOrder(long id)
        {
            try
            {
                var shoporder = db.ShopOrders.Find(id);
                shoporder.Status = 3;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CompletedOrder(long id)
        {
            try
            {
                var shoporder = db.ShopOrders.Find(id);
                shoporder.Status = 2;
                shoporder.DeliveryDate = DateTime.Now;
                db.SaveChanges();
                var total = new ShopOrderDAO().GetAllByIdShop(shoporder.IDMerchant);
                if(total.Count > 1)
                {
                    float countCompleted = total.Where(x=>x.Status==2).Count();
                    float sum = total.Count();
                    float Rate = (countCompleted / sum) * 100;
                    new SaleRateDAO().UpdateRate(shoporder.IDMerchant,(int)Rate);
                } 
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<ShopOrder> GetAllByIdShop(long? id)
        {
            return db.ShopOrders.Where(x => x.IDMerchant == id).ToList() ;
        }
    }
}
