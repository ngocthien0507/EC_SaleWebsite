using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.DAO
{
    public class advertisementDAO
    {
        TmdtDbContext db = null;
        public advertisementDAO()
        {
            db = new TmdtDbContext();
        }
        public void SetAdOutOfDate()
        {
            var date = DateTime.Now;
            var listad = db.advertisements.ToList();
            foreach (var item in listad)
            {
                if (item.ActiveDate.HasValue)
                {

                    if (date > item.EndDate)
                    {
                        item.Status = false;
                    }
                }
            }
            db.SaveChanges();
        }
        public long Insert(advertisement entity)
        {
            db.advertisements.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool Update(advertisement entity)
        {
            try
            {
                var ads = db.advertisements.Find(entity.ID);

                ads.Content = entity.Content;
                ads.Link = entity.Link;
                ads.Image = entity.Image;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //có thể làm ghi log
                return false;
            }
        }
        public List<advertisement> GetAvailableSlideAD()
        {
            var list = db.advertisements.Where(x => x.Location.Equals("SLIDE") && x.Status == true);
            return list.ToList();
        }
        public List<advertisement> GetAvailableSiteAD()
        {
            var ads = db.advertisements.Where(x => x.Status == true);
            var list = ads.Where(x => x.Location.Equals("LEFTAD") || x.Location.Equals("RIGHTAD"));
            return list.ToList();
        }
        public List<advertisement> GetByIdShop(long id)
        {
            return db.advertisements.Where(x => x.Merchant == id).ToList();
        }

        public advertisement GetDetail(long id)
        {
            return db.advertisements.Where(x => x.ID == id).SingleOrDefault();
        }
        public void ActiveAd(long id)
        {
            var ads = GetDetail(id);
            ads.Status = true;
            if(ads.ActiveDate.Value.Date <= DateTime.Now.Date)
            {
                ads.ActiveDate = DateTime.Now;
            }
            db.SaveChanges();
        }
        public IEnumerable<advertisement> ListAllPaging(string searchStatus,string searchName,long? searchID, string searchLocation, int page, int pageSize)
        {
            IQueryable<advertisement> model = db.advertisements;
            if (!string.IsNullOrEmpty(searchStatus))
            {
                if (searchStatus == "true")
                {
                    model = model.Where(x => x.Status == true);
                }
                else
                {
                    if(searchStatus == "3")
                    {
                        model = model.Where(x => x.OrderAdvertisements.Where(y => y.IDAd == x.ID).FirstOrDefault().Status==3 && x.Status==false);
                    }
                    else if(searchStatus == "0")
                    {
                        model = model.Where(x => x.OrderAdvertisements.Where(y => y.IDAd == x.ID).FirstOrDefault().Status == 0 && x.Status == false);
                    }
                    else
                    {
                        model = model.Where(x => (x.OrderAdvertisements.Where(y => y.IDAd == x.ID).FirstOrDefault().Status == 1 ||
                        x.OrderAdvertisements.Where(y => y.IDAd == x.ID).FirstOrDefault().Status == 2) && x.Status == false);
                    }
                }
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                model = model.Where(x => x.Content.Contains(searchName)
                                        || x.LocationAd.Name.Contains(searchName)
                                        || x.Account.Username.Contains(searchName));
            }
            if (!string.IsNullOrEmpty(searchLocation))
            {
                model = model.Where(x => x.Location.Equals(searchLocation));
            }
            if (searchID != null)
            {
                model = model.Where(x => x.ID == searchID);
            }
            return model.OrderBy(x => x.OrderAdvertisements.FirstOrDefault().Status ==3).
                ThenBy(x=>x.OrderAdvertisements.FirstOrDefault().Status == 2).ThenBy(x=> x.OrderAdvertisements.FirstOrDefault().Status == 1)
                .ThenBy(x=> x.OrderAdvertisements.FirstOrDefault().Status == 0)
                .ToPagedList(page, pageSize);
        }

    }
}
