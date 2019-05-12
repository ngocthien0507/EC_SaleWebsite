using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class RatingDAO
    {
        TmdtDbContext db = null;
        public RatingDAO()
        {
            db = new TmdtDbContext();
        }
        public long Insert(long id,int point)
        {
            var rating = new Rating();
            rating.IDMer = id;
            rating.Point = point;
            db.Ratings.Add(rating);
            db.SaveChanges();
            var quantityRating = GetAllbyIDMerchant(id);
            var Ratepoint = GetPointByIDMerchant(id);
            var user = new AccountDAO().GetInfoByID(id);
            if (quantityRating.Count == 5)
            {
                int? totalpoint = 0;
                foreach (var item in quantityRating)
                {
                    totalpoint += item.Point;
                }
                user.Rating = totalpoint / 5;
                new AccountDAO().UpdateUserRating(id, user.Rating);
            }
            else if(quantityRating.Count > 5)
            {
                user.Rating = ((quantityRating.Count() * Ratepoint) + point) / (quantityRating.Count() +1);
                new AccountDAO().UpdateUserRating(id, user.Rating);
            }
            return rating.ID;
        }
        public int? GetPointByIDMerchant(long idmer)
        {
            return db.Accounts.Where(x => x.ID == idmer).FirstOrDefault().Rating;
        }

        public List<Rating> GetAllbyIDMerchant(long idmer)
        {
           return db.Ratings.Where(x => x.IDMer == idmer).ToList();
        }
    }
}
