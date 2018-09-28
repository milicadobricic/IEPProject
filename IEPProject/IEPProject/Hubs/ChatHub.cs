using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace IEPProject.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message, double price)
        {
            Clients.All.addNewMessageToPage(name, message, price);
        }
    }
}