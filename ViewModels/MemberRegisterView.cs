using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class MemberRegisterView
    {
        public Members newMember { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }

        [DisplayName("確認密碼")]
        [Compare("Password",ErrorMessage ="兩次密碼輸入不一致")]
        [Required(ErrorMessage = "請輸入確認密碼")]
        public string PasswordCheck { get; set; }
    }
}