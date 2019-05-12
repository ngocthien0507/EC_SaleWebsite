using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using System.Data.Entity;

namespace Model.DAO
{
    public class BillAdDAO
    {
        TmdtDbContext db = null;
        public BillAdDAO()
        {
            db = new TmdtDbContext();
        }
        public long Insert(BillAd bill)
        {
            db.BillAds.Add(bill);
            db.SaveChanges();
            return bill.ID;
        }
        public List<BillAd> GetAll()
        {
            return db.BillAds.ToList();
        }
        public IEnumerable<BillAd> ListAllPaging(string searchString, DateTime? searchDate, long? searchID , int page = 1, int pageSize = 10)
        {
            IQueryable<BillAd> model = db.BillAds;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.NameCus.Contains(searchString)
                                        || x.Phone.Contains(searchString)
                                        || x.TradingCode.Equals(searchString));
            }
            if (searchID != null)
            {
                model = model.Where(x => x.ID == searchID || x.IdOrderAd == searchID);
            }
            if (searchDate != null)
            {
                model = model.Where(x => x.CreateDate.Value.Day == searchDate.Value.Day
                                    && x.CreateDate.Value.Month == searchDate.Value.Month
                                    && x.CreateDate.Value.Year == searchDate.Value.Year);
            }
            return model.OrderByDescending(x=>x.CreateDate).ToPagedList(page, pageSize);
        }
        public IEnumerable<BillAd> ListAll(string searchString, int page = 1, int pageSize = 3)
        {
            IQueryable<BillAd> model = db.BillAds;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.NameCus.Contains(searchString)
                                        || x.Phone.Contains(searchString)
                                        || x.TradingCode.Equals(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        public decimal? getMoneyByMonth(int? month,int? year)
        {
            decimal? totalmoney = 0;
            var listbill = GetAll();
            foreach(var item in listbill)
            {
                if(item.CreateDate.Value.Month==month && item.CreateDate.Value.Year == year)
                {
                    totalmoney += item.Money;
                }
            }
            return totalmoney;
        }
        public decimal? getMoneyByYear(int? year)
        {
            decimal? totalmoney = 0;
            var listbill = GetAll();
            foreach (var item in listbill)
            {
                if (item.CreateDate.Value.Year == year)
                {
                    totalmoney += item.Money;
                }
            }
            return totalmoney;
        }
        public decimal? GetTotalMoney()
        {
            decimal? totalmoney = 0;
            var listbill = GetAll();
            foreach (var item in listbill)
            {
                    totalmoney += item.Money;
            }
            return totalmoney;
        }
        public decimal? getMoneyByMerchant(string searchString)
        {
            decimal? totalmoney = 0;
            var listbill = GetAll();
            if (!string.IsNullOrEmpty(searchString))
            {
                foreach (var item in listbill)
                {
                    if (item.NameCus.Contains(searchString) || item.Phone.Equals(searchString) || item.TradingCode.Equals(searchString))
                    {
                        totalmoney += item.Money;
                    }
                }
            }
            return totalmoney;
        }
    }
}
