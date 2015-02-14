using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stripe;

namespace StripeWithCustomersAndCharges.Controllers
{
    public class CustomerController : Controller
    {
        List<Models.Product> products = new List<Models.Product>() { new Models.Product() {Description = "Prod1", Price = 100.00 }, 
                                                                     new Models.Product() {Description = "Prod2", Price = 400.00 },
                                                                     new Models.Product() {Description = "Prod3", Price = 125.00 } }; 

        // GET: Customer
        public ActionResult Index()
        {
            List<Models.Customer> customers = new List<Models.Customer>();

            var customerService = new StripeCustomerService();
            var stripeCustomers = customerService.List();

            foreach(var customer in stripeCustomers)
            {
                  var model = new Models.Customer();
                  model.ID = customer.Id;
                  model.Description = customer.Description;
                  model.Email = customer.Email;
                  customers.Add(model);
               //  customer.StripeCardList.StripeCards[0]
            }

              
            return View(customers);
        }

        // GET: Customer/Detail/
        public ActionResult Details(string id)
        {
            Models.Customer customer = new Models.Customer();
            List<Models.Card> cards = new List<Models.Card>();

            var customerService = new StripeCustomerService();

            // new StripeCustomerCreateOptions { }
            var stripeCustomer = customerService.Get(id);

            foreach(var card in stripeCustomer.StripeCardList.StripeCards)
            {
                var model = new Models.Card();
                model.ID = card.Id;
                model.LastFour = card.Last4;
                model.Brand = card.Brand;
                model.Line1 = card.AddressLine1;
                model.City = card.AddressCity;
                model.State = card.AddressState;
                model.CustomerID = card.CustomerId;
                cards.Add(model);
            }

            return View((cards.Count > 0) ? cards[0] : null);
        }


        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(Models.Customer model)
        {
            try
            {
                // TODO: Add insert logic here
                var customerService = new StripeCustomerService();

                var customerOptions = new StripeCustomerCreateOptions() { Email = model.Email, Description = model.Description };
                var stripeCustomer = customerService.Create(customerOptions);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Customer/Create
        public ActionResult CreateWithCard()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult CreateWithCard(Models.CustomerWithCard model)
        {
            try
            {
                var customerService = new StripeCustomerService();
                var customerOptions = new StripeCustomerCreateOptions() { Email = model.Email, Description = model.Description, TokenId=model.Token };
                var stripeCustomer = customerService.Create(customerOptions);

                var myCharge = new StripeChargeCreateOptions
                {
                    // convert the amount to pennies
                    Amount = (int)(model.Amount * 100),
                    Currency = "usd",
                    Description = "test charge with new customer and card",
                    // ReceiptEmail = "",   email not sent with test keys
                    // TokenId = model.Token Don't charge the card -- CHARGE the CUSTOMER
                    CustomerId = stripeCustomer.Id
                };

                var chargeService = new StripeChargeService();
                var stripeCharge = chargeService.Create(myCharge);   //Charge

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        // GET: Customer/Edit/5
        public ActionResult Edit(string id)
        {
            return View();
        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(Models.Customer model)
        {
            try
            {
                var customerService = new StripeCustomerService();
                var stripeCustomerUpdate = new StripeCustomerUpdateOptions() { Description = model.Description, Email = model.Email };
                customerService.Update(model.ID, stripeCustomerUpdate);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Delete/5
       
        public ActionResult Delete()
        {
                return View();
            
        }

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                // TODO: Add delete logic here
                var customerService = new StripeCustomerService();
                customerService.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Product(string id="")
        {
            ViewBag.CustomerID = id;
            return View();
        }


        public ActionResult Buy(string id = "", int price = 100)
        {

            var myCharge = new StripeChargeCreateOptions
            {
                // convert the amount to pennies
                Amount = price * 100,
                Currency = "usd",
                Description = "test charge to customer",
                // ReceiptEmail = "",   email not sent with test keys
                // TokenId = model.Token Don't charge the card -- CHARGE the CUSTOMER
                CustomerId = id
            };

            var chargeService = new StripeChargeService();
            var stripeCharge = chargeService.Create(myCharge);   //Charge


            return RedirectToAction("GetMyCharges", new { id = id });

        }

        [HttpGet]
        public ActionResult Charge()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Charge(Models.StripeModel stripeModel)
        {
           Stripe.StripeCharge stripePaymentResult = processPayment(stripeModel); // null
           return View("Product");  
        }

        public Stripe.StripeCharge processPayment(Models.StripeModel stripeModel)
        {
            try
            {
                var myCharge = new StripeChargeCreateOptions
                {
                    // convert the amount to pennies
                    Amount = (int)(stripeModel.Amount * 100),
                    Currency = "usd",
                    Description = "Description for test charge",
                    // ReceiptEmail = "",   email not sent with test keys
                    TokenId = stripeModel.Token
                };

                var chargeService = new StripeChargeService(); 
                var stripeCharge = chargeService.Create(myCharge);   //Charge

                return stripeCharge;
            }
            catch (StripeException exception)
            {
                switch (exception.StripeError.ErrorType)
                {
                    case "api_error":
                        Console.WriteLine(exception.Message);
                        break;
                    case "invalid_request_error":
                        Console.WriteLine(exception.Message);
                        break;
                    case "card_error":
                        Console.WriteLine(exception.Message);
                        // error codes: exception.StripeError.Code
                        //switch (exception.StripeError.Code)
                      
                        break;
                    default:
                        throw;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {

            }
            return null;
        }

        [HttpGet]
        public ActionResult GetMyCharges(string id)
        {
            var chargeService = new StripeChargeService();

            var chargeOptions = new StripeChargeListOptions();
            chargeOptions.Limit = 100;
            chargeOptions.CustomerId = id;

            var stripeCharge = chargeService.List(chargeOptions);

            return View(stripeCharge);
        }

   
        public void Refund(string id)
        {
            var refundChargeId = "";
            ViewBag.Refunded = true;

            try
            {
                var chargeService = new StripeChargeService();

                var stripeCharge = chargeService.List();
                var refundCharge = chargeService.Refund(id);  //Refund it
                refundChargeId = refundCharge.Id;
            }
            catch (StripeException ex)
            {
                Console.WriteLine(ex.Message);

                ViewBag.Refunded = false;
                refundChargeId = "";
            }

            RedirectToRoute("GetAllCharges");
            //return View("GetAllCharges");
        }


    }
}
