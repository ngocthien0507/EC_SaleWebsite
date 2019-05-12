using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMDT.Common
{
    [Serializable]
    public class UserLogin
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public int Level { get; set; }
    }
}