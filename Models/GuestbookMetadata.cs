using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    [MetadataType(typeof(GuestbookMetadata))]
    public partial class Guestbooks
    {
        private class GuestbookMetadata
        {
            [DisplayName("編號：")]
            public int Id { get; set; }

            [DisplayName("帳號：")]
            [Required(ErrorMessage = "請輸入帳號")]
            [StringLength(20, ErrorMessage = "帳號不可超過20字元")]
            public string MembersAccount { get; set; }

            [DisplayName("留言內容：")]
            [Required(ErrorMessage = "請輸入留言內容")]
            [StringLength(20, ErrorMessage = "留言內容不可超過20字元")]
            public string Content { get; set; }

            [DisplayName("新增時間：")]
            public System.DateTime CreateTime { get; set; }

            [DisplayName("回覆內容：")]
            [StringLength(20, ErrorMessage = "回覆內容不可超過100字元")]
            public string Reply { get; set; }

            [DisplayName("回覆時間：")]
            public Nullable<DateTime> ReplyTime { get; set; }
        }
    }
}