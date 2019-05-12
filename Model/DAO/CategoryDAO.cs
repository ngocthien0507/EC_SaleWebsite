using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class CategoryDAO
    {
        TmdtDbContext db = null;
        public CategoryDAO()
        {
            db = new TmdtDbContext();
        }
    }
}
