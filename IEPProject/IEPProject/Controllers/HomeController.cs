using IEPProject.Data_Models;
using IEPProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using IEPProject.Utilities;

namespace IEPProject.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult Parameters()
        {
            if (!User.Identity.GetApplicationUser().IsAdmin)
            {
                return HttpNotFound();
            }

            ViewBag.Currencies = db.Currencies.Select(c => c.Name).ToList();
            return View(db.Parameters.First());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Parameters(PortalParameters model)
        {
            if (!User.Identity.GetApplicationUser().IsAdmin)
            {
                return HttpNotFound();
            }

            if (!(model.S < model.G && model.G < model.P))
            {
                ViewBag.Currencies = db.Currencies.Select(c => c.Name).ToList();
                ViewBag.StatusMessage = "S < G < P must hold";
                return View(db.Parameters.First());
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Currencies()
        {
            if (!User.Identity.GetApplicationUser().IsAdmin)
            {
                return HttpNotFound();
            }

            return View(db.Currencies.OrderBy(c => c.Name).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Currencies(Currency model)
        {
            if (!User.Identity.GetApplicationUser().IsAdmin)
            {
                return HttpNotFound();
            }

            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Parameters");
        }

        public ActionResult CentiliCallback(int clientid, string status)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var order = db.Orders.Find(clientid);
            var subject = "Transaction ";
            if (status == "success")
            {
                subject += "succeeded!";
                order.State = OrderState.COMPLETED;
                db.Entry(order).State = EntityState.Modified;
                user.NumTokens += order.NumTokens;
                db.Entry(user).State = EntityState.Modified;
            }
            else
            {
                subject += "failed";
                order.State = OrderState.CANCELED;
                db.Entry(order).State = EntityState.Modified;
            }

            order.CompletionTime = DateTime.Now;

            db.SaveChanges();

            var email = user.Email; 
            var content = MailSender.OrderToString(order, status);
            MailSender.SendMail(email, user.FirstName, user.LastName, status == "success", order.NumTokens);

            return RedirectToAction("Index", "Manage");
        }

        public ActionResult Chat()
        {
            return View();
        }
    }
}
