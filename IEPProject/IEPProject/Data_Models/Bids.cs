using IEPProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IEPProject.Data_Models
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }

        public virtual Auction Auction { get; set; }

        public virtual ApplicationUser User { get; set; }

        public DateTime Time { get; set; }

        public BidState State { get; set; }
    }
}