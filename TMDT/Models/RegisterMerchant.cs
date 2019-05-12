using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMDT.Models
{
    public class RegisterMerchant
    {

        [StringLength(50)]
        public string ShopName { get; set; }

        [StringLength(12)]
        public string CMND { get; set; }

        [StringLength(200)]
        public string ImgCMND { get; set; }

        [StringLength(50)]
        public string ShopAddress { get; set; }
    }
}