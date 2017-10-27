using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;

namespace WebApplication3.Services
{
    public class GuestbooksDBService
    {
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        GuestbooksEntities db = new GuestbooksEntities();

        public List<Guestbooks> GetDataList(ForPaging Paging,string Search)
        {
            try
            {
                IQueryable<Guestbooks> SearchData;
                if (String.IsNullOrEmpty(Search))
                {
                    SearchData = GetAllDataList(Paging);
                }
                else
                {
                    SearchData = GetAllDataList(Paging, Search);
                }
                return SearchData.OrderByDescending(p => p.Id).Skip((Paging.NowPage - 1) * Paging.ItemNum).Take(Paging.ItemNum).ToList();
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public IQueryable<Guestbooks> GetAllDataList (ForPaging Paging)
        {
            try
            {
                IQueryable<Guestbooks> Data = db.Guestbooks;
                Paging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Data.Count() / Paging.ItemNum)));
                Paging.SetRightPage();
                return Data;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public IQueryable<Guestbooks> GetAllDataList(ForPaging Paging, string Search)
        {
            try
            {
                IQueryable<Guestbooks> Data = db.Guestbooks.Where(p => p.MembersAccount.Contains(Search) || p.Content.Contains(Search) || p.Reply.Contains(Search) || p.Members.Name.Contains(Search));
                Paging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Data.Count() / Paging.ItemNum)));
                Paging.SetRightPage();
                return Data;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public void InsertGuestbooks(Guestbooks newData)
        {
            try
            {
                newData.CreateTime = DateTime.Now.AddHours(8);
                db.Guestbooks.Add(newData);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error(LogUtility.GetExceptionDetails(ex));
            }
        }

        public Guestbooks GetDataById(int Id)
        {
            try
            {
                return db.Guestbooks.Find(Id);
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public void UpdateGuestbooks(Guestbooks UpdateData)
        {
            try
            {
                Guestbooks OldData = db.Guestbooks.Find(UpdateData.Id);
                OldData.MembersAccount = UpdateData.MembersAccount;
                OldData.Content = UpdateData.Content;
                db.SaveChanges();
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return;
            }
        }

        public void ReplyGuestbooks(Guestbooks ReplyData)
        {
            try
            {
                Guestbooks OldData = db.Guestbooks.Find(ReplyData.Id);
                OldData.Reply = ReplyData.Reply;
                OldData.ReplyTime = DateTime.Now.AddHours(8);
                db.SaveChanges();
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return;
            }
        }

        public bool CheckUpdate(int id)
        {
            try
            {
                Guestbooks Data = db.Guestbooks.Find(id);
                return (Data != null && Data.ReplyTime == null);
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return false;
            }
        }

        public void Deleteuestbooks(int Id)
        {
            try
            {
                Guestbooks DeleteData = db.Guestbooks.Find(Id);
                db.Guestbooks.Remove(DeleteData);
                db.SaveChanges();
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return;
            }
        }
    }
}