using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMDT.Models
{
    public class RegisterModel
    {
        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(20,MinimumLength =6,ErrorMessage = "Mật khẩu ít nhất 6 kí tự")]
        public string Password { get; set; }

        [Compare("Password",ErrorMessage = "Nhập lại mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

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