using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ContactDAO
    {
        TmdtDbContext db = null;
        public ContactDAO()
        {
            db = new TmdtDbContext();
        }
        public Contact GetContact()
        {
            return db.Contacts.SingleOrDefault(x => x.Status == true);
        }
    }
}
