using IEPProject.Data_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IEPProject.Models
{
    public class CreateAuction
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Product photo")]
        public HttpPostedFileBase UploadedPhoto { get; set; }

        [Required]
        [Display(Name = "Duration in seconds")]
        public int Duration { get; set; }

        [Required]
        [Display(Name = "Start price")]
        public double StartPrice { get; set; }

        [Display(Name = "Currency")]
        public string CurrencyName { get; set; }
    }

    public class CreateBid
    {
        public int AuctionId { get; set; }

        [Display(Name = "Bid price")]
        public double Price { get; set; }

        public string ReturnPage { get; set; }
    }

    public class ApproveAuction
    {
        public int AuctionId { get; set; }
    }

    public class SearchAuctions
    {
        [Display(Name = "Search query")]
        public string Query { get; set; }

        [Display(Name = "Minimum price")]
        public double? MinPrice { get; set; }

        [Display(Name = "Maximum price")]
        public double? MaxPrice { get; set; }

        [Display(Name = "State")]
        public AuctionState? State { get; set; }
    }
}