using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class SystemConfigDAO
    {
        TmdtDbContext db = null;
        public SystemConfigDAO()
        {
            db = new TmdtDbContext();
        }

        public SystemConfig GetByID(string id)
        {
            return db.SystemConfigs.SingleOrDefault(x => x.ID == id && x.Status == true);
        }
    }
}
