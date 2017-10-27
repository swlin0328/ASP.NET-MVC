using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Services;
using WebApplication3.ViewModels;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class GuestbookController : Controller
    {
        GuestbooksDBService guestbookService = new GuestbooksDBService();
        // GET: Guestbook
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDataList(string Search, int Page = 1)
        {
            GuestbookView Data = new GuestbookView();
            Data.Search = Search;
            Data.Paging = new ForPaging(Page);
            Data.DataList = guestbookService.GetDataList(Data.Paging, Data.Search);
            return PartialView(Data);
        }

        [HttpPost]
        public ActionResult GetDataList([Bind(Include = "Search")]GuestbookView Data)
        {
            return RedirectToAction("GetDataList", new { Search = Data.Search});
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add([Bind(Include = "Content")] Guestbooks Data)
        {
            Data.MembersAccount = User.Identity.Name;
            guestbookService.InsertGuestbooks(Data);
            return RedirectToAction("Index");
        }

         public ActionResult Edit(int Id)
        {
            Guestbooks Data = guestbookService.GetDataById(Id);
            return View(Data);
        }

        [HttpPost]
        public ActionResult Edit(int Id,[Bind(Include = "Content")] Guestbooks UpdateData)
        {
            if (guestbookService.CheckUpdate(Id))
            {
                UpdateData.Id = Id;
                UpdateData.MembersAccount = User.Identity.Name;
                guestbookService.UpdateGuestbooks(UpdateData);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Reply(int Id)
        {
            Guestbooks Data = guestbookService.GetDataById(Id);
            return View(Data);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Reply(int Id, [Bind(Include = "Reply")] Guestbooks ReplyData)
        {
            if (guestbookService.CheckUpdate(Id))
            {
                ReplyData.Id = Id;
                guestbookService.ReplyGuestbooks(ReplyData);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int Id)
        {
            guestbookService.Deleteuestbooks(Id);
            return RedirectToAction("Index");
        }
    }
}