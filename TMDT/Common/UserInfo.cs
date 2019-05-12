using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMDT.Common
{
    public class UserInfo
    {
        public long ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public bool Status { get; set; }

        public int? Level { get; set; }

        public long? IDMerchant { get; set; }

        public bool StatusShop { get; set; }
    }
}