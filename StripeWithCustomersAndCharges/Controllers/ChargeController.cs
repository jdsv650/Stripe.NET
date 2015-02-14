using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stripe;

namespace StripeWithCustomersAndCharges.Controllers
{
    public class ChargeController : Controller
    {

        // GET: Charge
        public ActionResult Index(int byDays = 1000, string resultMessage = null, string sortBy = "Amount", string searchBy = "")   //charge within last 1000 days
        {
            List<Stripe.StripeCharge> stripeCharge = new List<Stripe.StripeCharge>();

            try
            {
                var chargeService = new StripeChargeService();
                var chargeOptions = new StripeChargeListOptions();
                chargeOptions.Limit = 100;
                chargeOptions.Created = new StripeDateFilter { GreaterThanOrEqual = DateTime.UtcNow.Date.AddDays(-byDays) };

                stripeCharge = chargeService.List(chargeOptions).ToList();

                // Filter amount greater than passed in value
                if (!String.IsNullOrEmpty(searchBy))
                {
                    int num = 0;

                    if (Int32.TryParse(searchBy, out num))
                    {
                        num *= 100;
                        stripeCharge = stripeCharge.Where(c => c.Amount > num).ToList();
                    }

                }
                switch (sortBy)
                {
                    case "Created":
                        stripeCharge = stripeCharge.OrderBy(c => c.Created).ToList();
                        break;
                    default:
                        stripeCharge = stripeCharge.OrderByDescending(c => c.Amount).ToList();
                        break;
                }

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
                        break;
                }
                return RedirectToAction("Index", "Home", new { resultMessage = string.Format("Error: {0}", exception.Message) });

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return RedirectToAction("Index", "Home", new { resultMessage = string.Format("Error: {0}", exception.Message) });
            }
            ViewBag.SortBy = sortBy;

            return View(stripeCharge);

        }

        // GET: Charge/Details/5
        public ActionResult Details(string id)
        {
            Stripe.StripeCharge detailedCharge = new Stripe.StripeCharge();

            try
            {
                var chargeService = new StripeChargeService();
                var stripeCharge = chargeService.List();
                detailedCharge = chargeService.Get(id);
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
                        break;
                }
                return RedirectToAction("Index", "Home", new { resultMessage = string.Format("Error: {0}", exception.Message) });

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return RedirectToAction("Index", "Home", new { resultMessage = string.Format("Error: {0}", exception.Message) });
            }

            return View(detailedCharge);
        }


        // GET: Charge/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Charge/Create
        [HttpPost]
        public ActionResult Create(Models.StripeModel stripeModel)
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
                        break;
                }
                return RedirectToAction("Index", "Home", new { resultMessage = string.Format("Error: {0}", exception.Message) });

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return RedirectToAction("Index", "Home", new { resultMessage = string.Format("Error: {0}", exception.Message) });
            }

            return RedirectToAction("Index");

        }


        // GET: Charge/Edit/5
        public ActionResult Edit(string id)
        {
            return View();
        }

        // POST: Charge/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Models.Product model)
        {
            try
            {
                // TODO: Add update logic here
                var chargeService = new StripeChargeService();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Charge/Delete/5
        public ActionResult Delete(string id)
        {
            StripeCharge detailedCharge = new StripeCharge();

            try
            {
                var chargeService = new StripeChargeService();
                var stripeCharge = chargeService.List();
                detailedCharge = chargeService.Get(id);
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
                        break;
                }
                return RedirectToAction("Index", "Home", new { resultMessage = string.Format("Error: {0}", exception.Message) });

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return RedirectToAction("Index", "Home", new { resultMessage = string.Format("Error: {0}", exception.Message) });
            }

            return View(detailedCharge);
        }

        // POST: Charge/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Stripe.StripeCharge model)
        {
            try
            {
                var chargeService = new StripeChargeService();
                chargeService.Refund(id);

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
                        break;
                }
                return RedirectToAction("Index", "Home", new { resultMessage = string.Format("Error: {0}", exception.Message) });

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return RedirectToAction("Index", "Home", new { resultMessage = string.Format("Error: {0}", exception.Message) });
            }

            return RedirectToAction("Index");
        }


    }

}
