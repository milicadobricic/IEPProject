using IEPProject.Data_Models;
using IEPProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEPProject.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
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
    }
}