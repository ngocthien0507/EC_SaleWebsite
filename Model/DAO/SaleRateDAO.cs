using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class SaleRateDAO
    {
        TmdtDbContext db = null;
        public SaleRateDAO()
        {
            db = new TmdtDbContext();
        }
        public bool Insert(SaleRate rate)
        {
            try
            {
                db.SaleRates.Add(rate);
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool UpdateRate(long? id, int Rate)
        {
            try
            {
                var salerate = db.SaleRates.Find(id);
                salerate.Rate = Rate;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
