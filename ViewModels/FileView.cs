using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;
using WebApplication3.Services;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class FileView
    {
        public List<FileContent> FileList { get; set; }
        [DisplayName("檔案名稱")]
        [Required(ErrorMessage = "請輸入檔案名稱")]
        public string FileName { get; set; }
        public ForPaging Paging { get; set; }
    }
}