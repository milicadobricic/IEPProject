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

namespace IEPProject.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var ctx = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<Hubs.PriceChangeHub>();
            ctx.Clients.All.addNewMessageToPage("abc", "def", "ghi");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Parameters()
        {
            ViewBag.Currencies = db.Currencies.Select(c => c.Name).ToList();
            return View(db.Parameters.First());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Parameters(PortalParameters model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Currencies()
        {
            return View(db.Currencies.OrderBy(c => c.Name).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Currencies(Currency model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Parameters");
        }

        public ActionResult CentiliCallback(int clientid, string status)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var order = db.Orders.Find(clientid);
            if (status == "success")
            {
                order.State = OrderState.COMPLETED;
                db.Entry(order).State = EntityState.Modified;
                user.NumTokens += order.NumTokens;
                db.Entry(user).State = EntityState.Modified;
            }
            else
            {
                order.State = OrderState.CANCELED;
                db.Entry(order).State = EntityState.Modified;
            }

            db.SaveChanges();

            return RedirectToAction("Index", "Manage");
        }

        public ActionResult Chat()
        {
            return View();
        }
    }
}
