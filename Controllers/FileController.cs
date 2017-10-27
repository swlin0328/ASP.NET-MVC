using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.ViewModels;
using WebApplication3.Services;
using System.IO;
using WebApplication3.Models;
using System.Net;

namespace WebApplication3.Controllers
{
    public class FileController : Controller
    {
        FileService fileService = new FileService();

        // GET: File
        public ActionResult Index(int Page = 1)
        {
            FileView Data = new FileView();
            Data.Paging = new ForPaging(Page);
            Data.FileList = fileService.GetFileList(Data.Paging);
            return View(Data);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UploadFile(HttpPostedFileBase upload, string FileName)
        {
            if (upload != null)
            {
                //string filename = Path.GetFileName(upload.FileName);
                //string Url = Path.Combine(Server.MapPath("~/Upload/"), filename);
                //upload.SaveAs(Url);

                string Url = "https://uploadmvc.blob.core.windows.net/mvc/Upload/" + upload.FileName;
                FileService.FileRepository AzureUpload = new FileService.FileRepository();
                AzureUpload.UploadFileAzure("mvc", "Upload", upload);
                fileService.UploadFile(FileName, Url, upload.ContentLength, upload.ContentType);

                //fileService.UploadFile(FileName, Url, upload.ContentLength, upload.ContentType);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult DownloadFile(int Id)
        {
            FileContent Download = fileService.GetFileContent(Id);
            if (Download != null)
            {
                WebClient webClient = new WebClient();
                MemoryStream ms = new MemoryStream(webClient.DownloadData(Download.Url));
                return File(ms, Download.Type, Download.Name + Download.Url.Substring(Download.Url.LastIndexOf(".")));

                //Stream iStream = new FileStream(Download.Url, FileMode.Open, FileAccess.Read, FileShare.Read);
                //return File(iStream, Download.Type, Download.Name + Download.Url.Substring(Download.Url.LastIndexOf(".")));
            }
            else
            {
                return JavaScript("alert('無此檔案')");
            }
        }

        [Authorize]
        public ActionResult DeleteFile(int Id)
        {
            fileService.DeleteFile(Id);

            return RedirectToAction("Index");
        }
    }
}