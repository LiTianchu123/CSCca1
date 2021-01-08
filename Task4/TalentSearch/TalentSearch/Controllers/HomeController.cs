using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace TalentSearch.Controllers
{
    public class HomeController : Controller
    {

   
        public ActionResult Index()
        {
            return View();
        }


    }
}