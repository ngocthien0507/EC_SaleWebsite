using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.DAO
{
    public class AccountDAO
    {
        TmdtDbContext db = null;
        // Normal Part
        public AccountDAO()
        {
            db = new TmdtDbContext();
        }
        public void ResetPassWord(string pass,string email)
        {
            var user = GetAccountByEmail(email);
            user.Password = pass;
            db.SaveChanges();
        }
        public long Insert(Account entity)
        {
            db.Accounts.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool Update(Account entity)
        {
            try
            {
                var user = db.Accounts.Find(entity.ID);
                if (!String.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.Name = entity.Name;
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.Phone = entity.Phone;
                user.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //có thể làm ghi log
                return false;
            }
        }
        public Account GetAccountByEmail(string Email)
        {
            return db.Accounts.Where(x => x.Email.Equals(Email)).SingleOrDefault();
        }
        public void UpdateUserRating(long id, int? point)
        {
            var user = db.Accounts.Find(id);
            user.Rating = point;
            db.SaveChanges();
        }
        public bool Delete(long id)
        {
            try
            {
                var user = db.Accounts.Find(id);
                db.Accounts.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool EditInfo(Account entity, string Pass)
        {
            try
            {
                var account = db.Accounts.Find(entity.ID);
                if (!String.IsNullOrEmpty(Pass))
                {
                    account.Password = Pass;
                }
                account.Name = entity.Name;
                account.Address = entity.Address;
                account.Phone = entity.Phone;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //finding by...
        public Account GetInfoByUsername(string username)
        {
            return db.Accounts.SingleOrDefault(x => x.Username == username);
        }
        public Account FindByUsername(string username)
        {
            var row = db.Accounts.Where(x => x.Username.Equals(username));
            return row.SingleOrDefault();
        }
        public Account GetInfoByID(long id)
        {
            return db.Accounts.Find(id);
        }
        public List<Account> GetAllHaveShop()
        {
            return db.Accounts.Where(x => x.Level == 1).ToList();
        }

        //finished fiding by..

        //check register
        public bool IsExitsEmail(string email)
        {
            var row = db.Accounts.SingleOrDefault(x => x.Email == email);
            if (row == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool IsExitsPhone(string phone)
        {
            var row = db.Accounts.SingleOrDefault(x => x.Phone == phone);
            if (row == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //finished check register

        // đang nhập
        public int Login(string userName, string passWord)
        {
            var result = db.Accounts.SingleOrDefault(x => x.Username == userName);

            if (result == null)
            {
                return 0; // account không tồn tại
            }
            else
            {
                if (result.Password == passWord)
                {
                    return 1; //mật khảu chính xác
                }
                else
                {
                    return -2; //sai mật khẩu
                }
            }
        }

        //update tài khoản sau khi đăng ký gì đó
        public bool UpdateStatusUser(string username)
        {
            try
            {
                var user = GetInfoByUsername(username);
                user.Status = true;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateLevel(string username)
        {
            try
            {
                var user = GetInfoByUsername(username);
                user.Level = 1;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        // phân trang
        public IEnumerable<Account> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Account> model = db.Accounts;
            if (!string.IsNullOrEmpty(searchString))
            {
                if(searchString.Equals("NomalUser"))
                {
                    model = model.Where(x => x.Level == 0);
                }
                else if(searchString.Equals("MerchantUser"))
                {
                    model = model.Where(x => x.Level == 1 || x.Level == 2);
                }
                else if (searchString.Equals("ListAll"))
                {
                    model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
                }
                else
                {
                    model = model.Where(x => x.Username.Contains(searchString) || x.Name.Contains(searchString));

                }
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }



        //Merchant Part

        public List<Account> GetAllMerchant()
        {
            return db.Accounts.Where(x => x.Level == 1 || x.Level == 2).ToList();
        }
        //public Merchant GetMerchantByID(long id)
        //{
        //    return db.Merchants.SingleOrDefault(x => x.ID == id);
        //}
        //public bool GetStatusMerchantByID(long id)
        //{
        //    return db.Merchants.SingleOrDefault(x => x.ID == id).Status;
        //}

        //public long Insert(Merchant entity)
        //{
        //    db.Merchants.Add(entity);
        //    db.SaveChanges();
        //    return entity.ID;
        //}

        public bool UpdateMerchant(Account entity)
        {
            try
            {
                var user = db.Accounts.Find(entity.ID);
                user.ShopName = entity.ShopName;
                user.ShopAddress = entity.ShopAddress;
                user.CMND = entity.CMND;
                if (entity.Rating != null)
                {
                    user.Rating = entity.Rating;
                }
                if (entity.ImgCMND != null)
                {
                    user.ImgCMND = entity.ImgCMND;
                }
                user.CreateDateShop = entity.CreateDateShop;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //có thể làm ghi log
                return false;
            }
        }

        public bool BlockMerchant(long id)
        {
            try
            {
                var user = db.Accounts.Find(id);
                user.StatusShop = false;
                user.Level = 1;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateStatusMerchant(long id)
        {
            try
            {
                var user = GetInfoByID(id);
                user.StatusShop = true;
                user.Level = 2;
                var rate = new SaleRate();
                rate.ID = id;
                rate.Rate = 0;
                var salerate = new SaleRateDAO().Insert(rate);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Account> ListAllPagingMerchant(string searchString, int page, int pageSize)
        {
            IQueryable<Account> model = db.Accounts.Where(x => x.Level == 1 || x.Level == 2);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.ShopName.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
    }
}