using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using IEPProject.Data_Models;
using IEPProject.Models;
using System.IO;
using IEPProject.Utilities;
using System.Web.Helpers;

namespace IEPProject.Controllers
{
    public class AuctionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Auctions
        public ActionResult Index()
        {
            return View(db.Auctions.ToList());
        }

        // GET: Auctions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        // GET: Auctions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auctions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateAuction auction)
        {
            if (ModelState.IsValid)
            {
                var imagePath = string.Concat("~/Images/", FileNameGenerator.generate());
                var path = Server.MapPath(imagePath);
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                Auction auctionModel = new Auction(auction, string.Concat(imagePath.Substring(1), ".jpeg"), user);
                WebImage img = new WebImage(auction.UploadedPhoto.InputStream);
                img.Resize(256, 256, true, true);
                img.Save(path);

                db.Auctions.Add(auctionModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(auction);
        }

        // GET: Auctions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        // POST: Auctions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ImagePath,Duration,StartPrice,CurrentPrice,CreationTime,OpeningTime,ClosingTime,State")] Auction auction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(auction);
        }

        // GET: Auctions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        // POST: Auctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Auction auction = db.Auctions.Find(id);
            db.Auctions.Remove(auction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public ActionResult CreateBid(CreateBid model)
        {
            Auction auction = db.Auctions.Find(model.AuctionId);

            if(model.Price <= auction.CurrentPrice || model.Price < auction.StartPrice)
            {
                ViewBag.MessageStatus = "Offered price must be greater than current!";
                return View("Details", auction);
            }

            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            /*if(userId == auction.Creator.Id)
            {
                ViewBag.MessageStatus = "You can't participate in your own auction!";
                return View("Details", auction);
            }*/

            var bid = new Bid
            {
                Auction = auction,
                User = user,
                Time = DateTime.Now,
                State = BidState.CURRENTLY_BEST,
                OfferedPrice = model.Price
            };

            foreach (var b in db.Bids.Where(b => b.Auction.Id == auction.Id).Where(b => b.State == BidState.CURRENTLY_BEST)) {
                /*if(b.User.Id == userId)
                {
                    ViewBag.MessageStatus = "Your previous offer is currently the biggest!";
                    return View("Details", auction);
                }*/
                b.State = BidState.UNSUCCESSFUL;
                db.Entry(b).State = EntityState.Modified;
            }

            db.Bids.Add(bid);
            auction.CurrentPrice = model.Price;
            db.Entry(auction).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = auction.Id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
