using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    [MetadataType(typeof(FileContentMetadata))]
    public partial class FileContent
    {
        private class FileContentMetadata
        {
            [DisplayName("編號：")]
            public int Id { get; set; }

            [DisplayName("檔名：")]
            public string Name { get; set; }

            [DisplayName("路徑：")]
            public string Url { get; set; }

            [DisplayName("新增時間：")]
            public System.DateTime CreateTime { get; set; }

            [DisplayName("大小(Byte)：")]
            public int Size { get; set; }
        }
    }
}