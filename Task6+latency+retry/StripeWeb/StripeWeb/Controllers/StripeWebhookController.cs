using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using StripeWeb.Data;

namespace StripeWeb.Controllers
{
    public class StripeWebhookController : Controller
    {

        public ApplicationDbContext Database { get; }
        public StripeWebhookController(
        ApplicationDbContext database)
        {
            Database = database;
        }

        // GET: StripeWebHook
        [HttpPost]
        public async Task<IActionResult> Index()
        {

            //using (Stream iStream = Request.InputStream)
            //{
            //    using (StreamReader reader = new StreamReader(iStream, Encoding.UTF8))   //you should use   Request.ContentEncoding
            //    {
            //        json = await reader.ReadToEndAsync();
            //    }
            //}
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {

                var stripeEvent = EventUtility.ParseEvent(json);
                if (stripeEvent.Type == Events.CustomerCreated)
                {
                    Customer cus = stripeEvent.Data.Object as Customer;

                    Models.Customer customer = new Models.Customer();
                    customer.CustomerID = cus.Id;
                    customer.Name = cus.Name;
                    customer.Email = cus.Email;
                    customer.Balance = (int)cus.Balance;
                    customer.CreatedAt = DateTime.Now;
                    customer.UpdatedAt = DateTime.Now;
                    try
                    {
                        Database.Add(customer);
                        Database.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        return BadRequest();
                    }

                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionCreated)
                {
                    Subscription sub = stripeEvent.Data.Object as Subscription;
                    Price price = sub.Items.Data[0].Price;
                    int amount = (int)price.UnitAmount;
                    string subscriptionId = sub.Items.Data[0].Subscription;


                    //Models.Customer customer = Database.Customers.SingleOrDefault(x => x.CustomerID == "cus_IhyVFRJRWlxfwh");
                    //customer.Balance = customer.Balance - amount;
                    //customer.UpdatedAt = DateTime.Now;

                    Models.Subscription subscription = new Models.Subscription();
                    subscription.SubscriptionID = subscriptionId;
                    subscription.CustomerID = "cus_IhyVFRJRWlxfwh";
                    subscription.UnitAmount = amount;
                    subscription.CreatedAt = DateTime.Now;
                    subscription.UpdatedAt = DateTime.Now;

                    //Models.Transaction transaction = new Models.Transaction();
                    //transaction.CustomerID = "cus_IhyVFRJRWlxfwh";
                    //transaction.Description = Events.CustomerSubscriptionCreated;
                    //transaction.Amount = amount;
                    //transaction.CreatedAt = DateTime.Now;

                    try
                    {
                        //Database.Customers.Update(customer);
                        Database.Add(subscription);
                        //Database.Add(transaction);
                        Database.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        return BadRequest();
                    }

                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionUpdated)
                {
                    Subscription sub = stripeEvent.Data.Object as Subscription;
                    Price price = sub.Items.Data[0].Price;
                    int amount = (int)price.UnitAmount;
                    string subscriptionId = sub.Items.Data[0].Subscription;
                    Models.Subscription subscription = Database.Subscriptions.SingleOrDefault(x => x.SubscriptionID == subscriptionId);

                    subscription.UnitAmount = amount;
                    subscription.UpdatedAt = DateTime.Now;


                    try
                    {
                        Database.Subscriptions.Update(subscription);
                        Database.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        return BadRequest();
                    }

                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
                {
                    Subscription sub = stripeEvent.Data.Object as Subscription;
                    string subscriptionId = sub.Items.Data[0].Subscription;
                    Models.Subscription subscription = Database.Subscriptions.SingleOrDefault(x => x.SubscriptionID == subscriptionId);
                    try
                    {
                        Database.Remove(subscription);
                        Database.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        return BadRequest();
                    }
                }
                else if (stripeEvent.Type == Events.PaymentMethodAttached)
                {
                    Models.PaymentMethod paymentMethod = new Models.PaymentMethod();
                    PaymentMethod method = stripeEvent.Data.Object as PaymentMethod;
                    paymentMethod.PaymentMethodID = method.Id;
                    paymentMethod.Brand = method.Card.Brand;
                    paymentMethod.Last4 = Int32.Parse(method.Card.Last4);
                    paymentMethod.CreatedAt = DateTime.Now;
                    paymentMethod.UpdatedAt = DateTime.Now;
                    paymentMethod.CustomerID = "cus_IhyVFRJRWlxfwh";
                    try
                    {
                        Database.Add(paymentMethod);
                        Database.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        return BadRequest();
                    }
                }
                else if (stripeEvent.Type == Events.PaymentMethodDetached)
                {
                    PaymentMethod method = stripeEvent.Data.Object as PaymentMethod;
                    string paymentMethodID = method.Id;
                    Models.PaymentMethod paymentMethod = Database.PaymentMethods.SingleOrDefault(x => x.PaymentMethodID == paymentMethodID);

                    try
                    {
                        Database.Remove(paymentMethod);
                        Database.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        return BadRequest();
                    }
                }
                else if (stripeEvent.Type == Events.PaymentMethodUpdated)
                {
                    PaymentMethod method = stripeEvent.Data.Object as PaymentMethod;
                    string paymentMethodID = method.Id;
                    Models.PaymentMethod paymentMethod = Database.PaymentMethods.SingleOrDefault(x => x.PaymentMethodID == paymentMethodID);

                    paymentMethod.Last4 = Int32.Parse(method.Card.Last4);
                    paymentMethod.Brand = method.Card.Brand;
                    paymentMethod.UpdatedAt = DateTime.Now;

                    try
                    {
                        Database.PaymentMethods.Update(paymentMethod);
                        Database.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        return BadRequest();
                    }
                }
                else if (stripeEvent.Type == Events.ChargeSucceeded)
                {

                    Charge charge = stripeEvent.Data.Object as Charge;
                    int amount = (int)charge.Amount;

                    Models.Customer customer = Database.Customers.SingleOrDefault(x => x.CustomerID == "cus_IhyVFRJRWlxfwh");
                    customer.Balance = customer.Balance - amount;
                    customer.UpdatedAt = DateTime.Now;

                    Models.Transaction transaction = new Models.Transaction();
                    transaction.CustomerID = "cus_IhyVFRJRWlxfwh";
                    transaction.Description = Events.ChargeSucceeded;
                    transaction.Amount = amount;
                    transaction.CreatedAt = DateTime.Now;

                    try
                    {
                        Database.Customers.Update(customer);
                        Database.Add(transaction);
                        Database.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        return BadRequest();
                    }
                }
                else if (stripeEvent.Type == Events.ChargeFailed)
                {
                    Charge charge = stripeEvent.Data.Object as Charge;
                    int amount = 0;
                    Models.Transaction transaction = new Models.Transaction();
                    transaction.CustomerID = "cus_IhyVFRJRWlxfwh";
                    transaction.Description = Events.ChargeFailed;
                    transaction.Amount = amount;
                    transaction.CreatedAt = DateTime.Now;
                    try
                    {
                        Database.Add(transaction);
                        Database.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        return BadRequest();
                    }
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}