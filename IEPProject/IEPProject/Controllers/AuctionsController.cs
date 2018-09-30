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
using LinqKit;
using Microsoft.AspNet.SignalR;
using IEPProject.Hubs;
using PagedList;

namespace IEPProject.Controllers
{
    public class AuctionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Auctions
        public ActionResult Index(int? id, string messageStatus, int? errorAuction, SearchAuctions model, int? page)
        {
            ViewBag.Query = model.Query;
            ViewBag.MinPrice = model.MinPrice;
            ViewBag.MaxPrice = model.MaxPrice;
            ViewBag.State = model.State;

            if (errorAuction != null)
            {
                ViewBag.MessageStatus = messageStatus;
                ViewBag.ErrorAuction = errorAuction;
            }

            if (model != null)
            {
                IQueryable<Auction> ret = db.Auctions;

                var numOfRows = db.Parameters.First().N;

                if (!string.IsNullOrWhiteSpace(model.Query))
                {
                    var parts = model.Query.Split(new[] { ' ', ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    var predicate = PredicateBuilder.New<Auction>(false);
                    foreach (var part in parts)
                    {
                        predicate = predicate.Or(p => p.Name.Contains(part));
                    }

                    ret = ret.Where(predicate);
                    //ret = ret.Where(a => a.Name.Contains(model.Query));
                }

                if (model.MinPrice != null)
                {
                    ret = ret.Where(a => a.CurrentPrice >= model.MinPrice);
                }

                if (model.MaxPrice != null)
                {
                    ret = ret.Where(a => a.CurrentPrice <= model.MaxPrice);
                }

                if (model.State != null)
                {
                    ret = ret.Where(a => a.State == model.State);
                }

                foreach (var entry in ret)
                {
                    if (entry.ClosingTime.HasValue)
                    {
                        entry.ClosingTime = entry.ClosingTime.Value.ToUniversalTime();
                    }
                }

                ret = ret.OrderByDescending(a => a.ClosingTime);

                var pageSize = 3 * numOfRows;
                var pageNumber = page ?? 1;

                return View(ret.ToPagedList(pageNumber, pageSize));
            }
            else {
                var ret = db.Auctions.Where(a => a.State == AuctionState.OPENED).OrderByDescending(a => a.ClosingTime).ToList();

                foreach (var entry in ret)
                {
                    if (entry.ClosingTime.HasValue)
                    {
                        entry.ClosingTime = entry.ClosingTime.Value.ToUniversalTime();
                    }
                }

                return View(ret);
            }
        }

        // GET: Auctions/Details/5
        public ActionResult Details(int? id, string messageStatus, int? errorAuction)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (errorAuction != null)
            {
                ViewBag.MessageStatus = messageStatus;
                ViewBag.ErrorAuction = errorAuction;
            }

            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }

            // db.Entry(auction).State = EntityState.Detached;

            if (auction.ClosingTime.HasValue)
            {
                auction.ClosingTime = auction.ClosingTime.Value.ToUniversalTime();
            }

            return View(auction);
        }

        // GET: Auctions/Create
        [System.Web.Mvc.Authorize]
        public ActionResult Create()
        {
            var parameters = db.Parameters.First();
            var model = new CreateAuction { Duration = parameters.D, CurrencyName = parameters.C };
            return View(model);
        }

        // POST: Auctions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Web.Mvc.Authorize]
        public ActionResult Create(CreateAuction auction)
        {
            if (ModelState.IsValid && auction.StartPrice > 0)
            {
                var currencyName = db.Parameters.First().C;
                var currencyValue = db.Currencies.Where(c => c.Name == currencyName).First().Value;
                auction.StartPrice /= currencyValue;

                string imagePath;
                string format;

                if (auction.UploadedPhoto == null)
                {
                    imagePath = "~/Images/default-image";
                    format = ".jpeg";
                }
                else
                {
                    imagePath = string.Concat("~/Images/", FileNameGenerator.generate());
                    try {
                        WebImage img = new WebImage(auction.UploadedPhoto.InputStream);
                        img.Resize(256, 256, true, true);
                        format = "." + img.ImageFormat;
                        var path = Server.MapPath(imagePath);
                        img.Save(path);
                    } catch (ArgumentException)
                    {
                        ViewBag.ErrorMessage = "Image format not valid";
                        return View(auction);
                    }
                }
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                Auction auctionModel = new Auction(auction, string.Concat(imagePath.Substring(1), format), user);

                db.Auctions.Add(auctionModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (auction.StartPrice <= 0)
            {
                ViewBag.ErrorMessage = "Start price must be greater than 0!";
            }

            return View(auction);
        }

        // GET: Auctions/Edit/5
        [System.Web.Mvc.Authorize]
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
        [System.Web.Mvc.Authorize]
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
        [System.Web.Mvc.Authorize]
        public ActionResult Delete(int? id)
        {
            if (!User.Identity.GetApplicationUser().IsAdmin)
            {
                return HttpNotFound();
            }

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
        [System.Web.Mvc.Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.Identity.GetApplicationUser().IsAdmin)
            {
                return HttpNotFound();
            }

            Auction auction = db.Auctions.Find(id);
            db.Auctions.Remove(auction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Web.Mvc.Authorize]
        public ActionResult CreateBid(CreateBid model)
        {

            Auction auction = db.Auctions.Find(model.AuctionId);

            if(auction.State != AuctionState.OPENED)
            {
                var status = "You can't bid on auctions that are not opened!";
                return RedirectToAction(model.ReturnPage, new { id = auction.Id, messageStatus = status, errorAuction = model.AuctionId });
            }

            if (model.Price <= auction.CurrentPrice || model.Price < auction.StartPrice)
            {
                var status = "Offered price must be greater than current!";
                return RedirectToAction(model.ReturnPage, new { id = auction.Id, messageStatus = status, errorAuction = model.AuctionId });
            }

            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var previous = db.Bids.Where(b => b.Auction.Id == auction.Id).Where(b => b.State == BidState.CURRENTLY_BEST).FirstOrDefault();
            var previousUser = previous?.User;

            if(userId == auction.Creator.Id)
            {
                var status = "You can't participate in your own auction!";
                return RedirectToAction(model.ReturnPage, new { id = auction.Id, messageStatus = status, errorAuction = model.AuctionId });
            }

            if (user.NumTokens < model.Price)
            {
                var status = "You don't have enough tokens!";
                return RedirectToAction(model.ReturnPage, new { id = auction.Id, messageStatus = status, errorAuction = model.AuctionId });
            }

            var bid = new Bid
            {
                Auction = auction,
                User = user,
                Time = DateTime.Now,
                State = BidState.CURRENTLY_BEST,
                OfferedPrice = model.Price
            };

            if (previous != null)
            {
                if(previousUser.Id == userId)
                {
                    var status = "Your previous offer is currently the biggest!";
                    return RedirectToAction(model.ReturnPage, new { id = auction.Id, messageStatus = status, errorAuction = model.AuctionId });
                }

                previousUser.NumTokens += previous.OfferedPrice;
                db.Entry(previousUser).State = EntityState.Modified;
                previous.State = BidState.UNSUCCESSFUL;
                db.Entry(previous).State = EntityState.Modified;
            }

            db.Bids.Add(bid);
            auction.CurrentPrice = model.Price;
            db.Entry(auction).State = EntityState.Modified;
            user.NumTokens -= model.Price;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            var context = GlobalHost.ConnectionManager.GetHubContext<PriceChangeHub>();
            context.Clients.All.updatePrice(auction.Id.ToString(), model.Price.ToString(), user.UserName.ToString());

            return RedirectToAction("Details", new { id = auction.Id });
        }

        [System.Web.Mvc.Authorize]
        public ActionResult Approve()
        {
            if (!User.Identity.GetApplicationUser().IsAdmin)
            {
                return HttpNotFound();
            }

            return View(db.Auctions.Where(a => a.State == AuctionState.READY).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Web.Mvc.Authorize]
        public ActionResult Approve(ApproveAuction model)
        {
            if (!User.Identity.GetApplicationUser().IsAdmin)
            {
                return HttpNotFound();
            }

            var auction = db.Auctions.Find(model.AuctionId);
            auction.State = AuctionState.OPENED;
            var now = DateTime.Now;
            auction.OpeningTime = now;
            auction.ClosingTime = now.AddSeconds(auction.Duration);
            db.Entry(auction).State = EntityState.Modified;
            db.SaveChanges();

            return Redirect("Approve");
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
