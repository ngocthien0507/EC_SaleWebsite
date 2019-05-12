using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class SlideDAO
    {
        TmdtDbContext db = null;
        public SlideDAO()
        {
            db = new TmdtDbContext();
        }
        public List<Slide> GetAll()
        {
            return db.Slides.Where(x =>x.Status == true).OrderBy(y=>y.DisplayOrder).ToList();
        }
    }
}
