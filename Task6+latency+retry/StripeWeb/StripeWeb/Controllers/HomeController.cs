using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.BillingPortal;
using StripeWeb.Data;
using StripeWeb.Models;

namespace StripeWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ApplicationDbContext Database { get; }
    
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext database)
        {
            Database = database;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

   

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public ActionResult CustomerPortal()
        {
            StripeConfiguration.ApiKey = "***************";
            Session session = new Session();
            try
            {
                var options = new SessionCreateOptions
                {
                    Customer = "cus_IhyVFRJRWlxfwh",
                    ReturnUrl = "https://localhost:44318/",
                };
                var service = new SessionService();
                session = service.Create(options);
            }
            catch {
                return BadRequest();
            }

            return Content(session.Url);
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            StripeConfiguration.ApiKey = "****************";
            List<object> resultList = new List<object>();
            try
            {
                var options = new PriceListOptions { Currency = "sgd" };
                var service = new PriceService();
                StripeList<Price> prices = service.List(options);
              
                foreach (Price p in prices)
                {

                    float amount = (float)p.UnitAmount / 100;
                    string id = p.Id;
                    resultList.Add(new
                    {
                        id = id,
                        amount = amount,
                    });
                }
            }
            catch {
                return BadRequest();
            }
            return Ok(new JsonResult(resultList));
        }

        [HttpGet]
        public IActionResult GetSubscriptionList()
        {
        
            StripeConfiguration.ApiKey = "*****************";
            List<object> resultList = new List<object>();
            try
            {
                var options = new SubscriptionListOptions
                {
                };
                var service = new SubscriptionService();
                StripeList<Stripe.Subscription> subscriptions = service.List(
                  options
                );
            
                foreach (Stripe.Subscription s in subscriptions)
                {

                    string id = s.Items.Data[0].Subscription;
                    float amount = (float)s.Items.Data[0].Plan.Amount / 100;
                    resultList.Add(new
                    {
                        id = id,
                        amount = amount,
                    });
                }
            }
            catch {
                return BadRequest();
            }
            return Ok(new JsonResult(resultList));

        }

        [HttpGet]
        public IActionResult GetBalance()
        {
            float balance = 0;
            try
            {
                Models.Customer customer = Database.Customers.SingleOrDefault(x => x.CustomerID == "cus_IhyVFRJRWlxfwh");
                balance = (float)customer.Balance / 100;
            }
            catch
            {
                return BadRequest();
            }
            return Content(balance.ToString());
        }

        [HttpPost]
        public IActionResult PostSubscription(string priceId)
        {
            StripeConfiguration.ApiKey = "***************";
            try
            {
                var options = new SubscriptionCreateOptions
                {
                    Customer = "cus_IhyVFRJRWlxfwh",
                    Items = new List<SubscriptionItemOptions>
                  {
                    new SubscriptionItemOptions
                  {
                        Price = priceId,
                  },
                },


                };
                var service = new SubscriptionService();
                service.Create(options);
            }
            catch {
                return BadRequest();
            }
            return Ok();
        }

    }
}
