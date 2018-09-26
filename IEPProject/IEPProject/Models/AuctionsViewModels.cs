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
        [Display(Name = "Duration")]
        public int Duration { get; set; }

        [Required]
        [Display(Name = "Start price")]
        public double StartPrice { get; set; }
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
}