using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class MemberLoginView
    {
        [DisplayName("會員帳號")]
        [Required(ErrorMessage = "請輸入會員帳號")]
        public string UserName { get; set; }

        [DisplayName("會員密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }
    }
}