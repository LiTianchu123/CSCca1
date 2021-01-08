using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageUpload.Controllers
{
    public class HomeController : Controller
    {
        //private const string keyName = "updatedtestfile.txt";
        //private const string filePath = null;
        // Specify your bucket region (an example region is shown).  
        private static readonly string bucketName = ConfigurationManager.AppSettings["BucketName"];
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast1;
        private static readonly string accesskey = ConfigurationManager.AppSettings["AWSAccessKey"];
        private static readonly string secretkey = ConfigurationManager.AppSettings["AWSSecretKey"];
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadImage()
        {

            return View();
        }

        //[HttpPost]
        //public ActionResult UploadFile(HttpPostedFileBase file)
        //{
        //    var s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);

        //    var fileTransferUtility = new TransferUtility(s3Client);
        //    try
        //    {
        //        if (file.ContentLength > 0)
        //        {
        //            var filePath = Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(file.FileName));
        //            var fileTransferUtilityRequest = new TransferUtilityUploadRequest
        //            {
        //                BucketName = bucketName,
        //                FilePath = filePath,
        //                StorageClass = S3StorageClass.StandardInfrequentAccess,
        //                PartSize = 6291456, // 6 MB.  
        //                Key = keyName,
        //                CannedACL = S3CannedACL.PublicRead
        //            };
        //            fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
        //            fileTransferUtilityRequest.Metadata.Add("param2", "Value2");
        //            fileTransferUtility.Upload(fileTransferUtilityRequest);
        //            fileTransferUtility.Dispose();
        //        }
        //        ViewBag.Message = "File Uploaded Successfully!!";
        //    }

        //    catch (AmazonS3Exception amazonS3Exception)
        //    {
        //        if (amazonS3Exception.ErrorCode != null &&
        //            (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
        //            ||
        //            amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
        //        {
        //            ViewBag.Message = "Check the provided AWS Credentials.";
        //        }
        //        else
        //        {
        //            ViewBag.Message = "Error occurred: " + amazonS3Exception.Message;
        //        }
        //    }
        //    return RedirectToAction("S3Sample");
        //}

        [HttpPost]
        public ActionResult UploadToS3(string base64String)
        {
            var client = new AmazonS3Client(accesskey, secretkey, bucketRegion);
            var itemUrl = "";
            byte[] bytes = Convert.FromBase64String(base64String);
            try
            {
                string keyName = string.Format("talent_image_" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + ".jpg");
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.PublicRead,
                    Key = keyName
                };


                using (var ms = new MemoryStream(bytes))
                {
                    request.InputStream = ms;
                    client.PutObject(request);
                }
                itemUrl = "https://" + bucketName + ".s3-ap-southeast-1.amazonaws.com/" + keyName;

            }
            catch (Exception e)
            {
                return Content("Error");
            }




            return Content(itemUrl);
        }
    }
}