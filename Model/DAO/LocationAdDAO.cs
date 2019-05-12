using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class LocationAdDAO
    {
        TmdtDbContext db = null;
        public LocationAdDAO()
        {
            db = new TmdtDbContext();
        }
        public List<LocationAd> GetAll()
        {
            return db.LocationAds.ToList();
        }
        public LocationAd GetDetail(string id)
        {
            return db.LocationAds.Where(x => x.ID.Equals(id)).SingleOrDefault();
        }
    }
}
