using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.DAO
{
    public class OrderAdvertisementDAO
    {
        TmdtDbContext db = null;
        public OrderAdvertisementDAO()
        {
            db = new TmdtDbContext();
        }
        public long Insert(OrderAdvertisement entity)
        {
            db.OrderAdvertisements.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool RefuseAdvertisement(long id)
        {
            try
            {
                var ads = db.OrderAdvertisements.Find(id);
                ads.Status = 2;
                ads.advertisement.Status = false;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool AcceptedAdvertisement(long id)
        {
            try
            {
                var ads = db.OrderAdvertisements.Find(id);
                ads.Status = 3;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<OrderAdvertisement> ListAllPaging(string searchName, string searchLocation, int page, int pageSize)
        {
            IQueryable<OrderAdvertisement> model = db.OrderAdvertisements;
            if (!string.IsNullOrEmpty(searchName))
            {
                model = model.Where(x => x.advertisement.Content.Contains(searchName)
                                        || x.advertisement.LocationAd.Name.Contains(searchName)
                                        || x.advertisement.Account.Username.Contains(searchName));
            }
            if (!string.IsNullOrEmpty(searchLocation))
            {
                model = model.Where(x => x.advertisement.Location.Equals(searchLocation));
            }
            return model.OrderBy(x => x.Status == 3).ThenBy(x => x.Status == 2)
                .ThenBy(x => x.Status == 1).ThenBy(x => x.Status == 0).ThenBy(x => x.CreateDate).ToPagedList(page, pageSize);
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
        public List<OrderAdvertisement> GetAllSiteAD()
        {
            return db.OrderAdvertisements.Where(x => x.advertisement.Location != "SLIDE" && x.Status != 2).ToList();
        }
        public List<OrderAdvertisement> GetByIdShop(long id)
        {
            return db.OrderAdvertisements.Where(x => x.IDmerchant == id).ToList();
        }

        public OrderAdvertisement GetDetail(long id)
        {
            return db.OrderAdvertisements.Find(id);
        }
        public long UpdateStatusComplete(long id)
        {
            try
            {
                var order = db.OrderAdvertisements.Find(id);
                order.Status = 1;
                db.SaveChanges();
                return (long)order.IDAd;
            }
            catch(Exception ex)
            {
                return 0 ;
            }
        }
    }
}
