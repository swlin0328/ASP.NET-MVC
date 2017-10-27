using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using System.IO;

namespace WebApplication3.Services
{
    public class FileService
    {
        FileContentEntities db = new FileContentEntities();
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void UploadFile (string FileName, string Url, int Size, string Type)
        {
            FileContent newFile = new FileContent();

            try
            {
                newFile.Name = FileName;
                newFile.Url = Url;
                newFile.Size = Size;
                newFile.Type = Type;
                newFile.CreateTime = DateTime.Now.AddHours(8);

                db.FileContents.Add(newFile);
                db.SaveChanges();
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return;
            }
        }

        public List<FileContent> GetFileList(ForPaging Paging)
        {
            try
            {
                IQueryable<FileContent> SearchData = GetAllFileList(Paging);
                return SearchData.OrderByDescending(p => p.Id).Skip((Paging.NowPage - 1) * Paging.ItemNum).Take(Paging.ItemNum).ToList();
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public IQueryable<FileContent> GetAllFileList(ForPaging Paging)
        {
            try
            {
                IQueryable<FileContent> Data = db.FileContents;
                Paging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Data.Count()) / Paging.ItemNum));
                Paging.SetRightPage();
                return Data;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public FileContent GetFileContent(int Id)
        {
            try
            {
                FileContent GetFile = db.FileContents.Find(Id);
                return GetFile;
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return null;
            }
        }

        public void DeleteFile(int Id)
        {
            FileContent GetFile = db.FileContents.Find(Id);

            try
            {
                //System.IO.File.Delete(GetFile.Url);

                //Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=uploadmvc;AccountKey=Jww05RChCYY5vL6qu3fYumb2vhVow44SGnXI1xuSK0QEEGKPKlfGW00XAo4qp3AhknzV74kbdtmJa7GFJ5g1Jw==;EndpointSuffix=core.windows.net");
                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference("mvc");
                // Retrieve reference to a blob named "[要刪除的檔案名稱]".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference("Upload/" + GetFile.Url.Substring(GetFile.Url.LastIndexOf("/") + 1));
                // Delete the blob.
                blockBlob.Delete();

                db.FileContents.Remove(GetFile);
                db.SaveChanges();
            }
            catch (System.IO.IOException e)
            {
                logger.Error(LogUtility.GetExceptionDetails(e));
                return;
            }
        }

        public interface IFileRepository
        {
            string UploadFileAzure(string containerName, string groupName, HttpPostedFileBase file);
        }

        public class FileRepository : IFileRepository
        {
            public string UploadFileAzure(string containerName, string groupName, HttpPostedFileBase file)
            {

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("azure.blob.connectionstring"));

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container. 
                CloudBlobContainer container = blobClient.GetContainerReference(containerName.ToLower());

                // Create the container if it doesn't already exist.
                container.CreateIfNotExists();

                container.SetPermissions(
                         new BlobContainerPermissions
                         {
                             PublicAccess =
                                 BlobContainerPublicAccessType.Blob
                         });

                var fileName = groupName + "/" + file.FileName;
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
                
                blockBlob.Properties.ContentType = file.ContentType;
                blockBlob.UploadFromStream(file.InputStream);

                return blockBlob.Uri.AbsoluteUri;
            }
        }

        public void UploadXLSAzure(string containerName, string groupName, string xlsName, MemoryStream stram)
        {

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("azure.blob.connectionstring"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference(containerName.ToLower());

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            container.SetPermissions(
                     new BlobContainerPermissions
                     {
                         PublicAccess =
                             BlobContainerPublicAccessType.Blob
                     });

            var fileName = groupName + "/" + xlsName;
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            blockBlob.UploadFromStream(stram);
            stram.Seek(0, SeekOrigin.Begin);
            blockBlob.Properties.ContentType = "application/octet-stream ";
            blockBlob.SetProperties();
            BlobRequestOptions options = new BlobRequestOptions();
            blockBlob.UploadFromStream(stram);
        }
    }
}