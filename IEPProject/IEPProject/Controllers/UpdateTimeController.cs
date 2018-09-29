using IEPProject.Data_Models;
using IEPProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IEPProject.Controllers
{
    public class UpdateTimeController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public string Get()
        {
            var finishedAuctions = db.Auctions.Where(a => a.State == AuctionState.OPENED && a.ClosingTime < DateTime.Now).ToList();
            foreach(var auction in finishedAuctions)
            {
                var bid = auction.Bids.Where(b => b.State == BidState.CURRENTLY_BEST).FirstOrDefault();
                if (bid != null)
                {
                    bid.State = BidState.SUCCESSFUL;
                    db.Entry(bid).State = EntityState.Modified;
                    auction.Creator.NumTokens += bid.OfferedPrice;
                    db.Entry(auction.Creator).State = EntityState.Modified;
                }

                auction.State = AuctionState.COMPLETED;
                db.Entry(auction).State = EntityState.Modified;
            }
            db.SaveChanges();

            return "Ok";
        }
    }
}
