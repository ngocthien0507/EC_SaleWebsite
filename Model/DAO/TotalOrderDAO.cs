using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.DAO
{
    public class TotalOrderDAO
    {
        TmdtDbContext db = null;
        public TotalOrderDAO()
        {
            db = new TmdtDbContext();
        }
        public long Insert(TotalOrder totaloder)
        {
            db.TotalOrders.Add(totaloder);
            db.SaveChanges();
            return totaloder.ID;
        }
        public bool UpdateStatus(long idtotal, int status)
        {
            try
            {
                var totalorder = GetDetail(idtotal);
                totalorder.Status = status;
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public List<TotalOrder> GetAll()
        {
            return db.TotalOrders.Where(x => x.Status != 3).ToList();
        }
        public TotalOrder GetDetail(long id)
        {
            return db.TotalOrders.Find(id);
        }
        public List<TotalOrder>GetTotalOrderById(long id)
        {
            return db.TotalOrders.Where(x => x.CustomerID == id).OrderByDescending(x=>x.CreateDate).ToList();
        }
        public TotalOrder RefuseOrder(long id)
        {
            var TotalOrder = db.TotalOrders.Find(id);
            TotalOrder.Status = 3;
            db.SaveChanges();
            return TotalOrder;
        }

        public IEnumerable<TotalOrder> ListAllPaging(long? searchID, string searchString, int page, int pageSize)
        {
            IQueryable<TotalOrder> model = db.TotalOrders;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.CustomerName.Contains(searchString)
                || x.Account.Name.Contains(searchString));
            }
            if (searchID.HasValue)
            {
                model = model.Where(x => x.ID == searchID);
            }
            return model.OrderBy(x => x.Status==3).ThenBy(x => x.Status==2).ThenBy(x => x.Status == 1).
                ThenBy(x=>x.Status==0).ToPagedList(page, pageSize);
        }
    }
}
