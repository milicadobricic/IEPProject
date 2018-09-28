using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace IEPProject.Hubs
{
    public class PriceChangeHub : Hub
    {
        public void Send(string auctionId, string price, string username)
        {
            Clients.All.updatePrice(auctionId, price, username);
        }
    }
}