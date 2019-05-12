using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class FooterDAO
    {
        TmdtDbContext db = null;
        public FooterDAO()
        {
            db = new TmdtDbContext();
        }
        
        public Footer GetFooter()
        {
            return db.Footers.SingleOrDefault(x=>x.Status == true);
        }
    }
}
