using IEPProject.Models;
using IEPProject.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IEPProject.Data_Models
{
    public class Auction
    {
        public Auction(){}

        public Auction(CreateAuction auction, string imagePath)
        {
            Name = auction.Name;
            ImagePath = imagePath;
            Duration = auction.Duration;
            StartPrice = auction.StartPrice;
            CurrentPrice = 0;
            CreationTime = DateTime.Now;
            State = AuctionState.READY;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string ImagePath { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public double StartPrice { get; set; }

        [Required]
        [Display(Name = "Current price")]
        public double CurrentPrice { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreationTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? OpeningTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ClosingTime { get; set; }

        [Required]
        public AuctionState State { get; set; }

        public virtual List<Bid> Bids { get; set; } = new List<Bid>();
    }
}
