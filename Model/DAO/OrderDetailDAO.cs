using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class OrderDetailDAO
    {
        TmdtDbContext db = null;
        public OrderDetailDAO()
        {
            db = new TmdtDbContext();
        }
        public bool Insert(OrderDetail detail)
        {
            try
            {
                db.OrderDetails.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<OrderDetail> ListByIdShopOrder(long id)
        {
            return db.OrderDetails.Where(x => x.IDShopOrder == id).ToList();
        }

    }
}
