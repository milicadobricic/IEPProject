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
        public int Duration { get; set; }

        [Required]
        public double StartPrice { get; set; }
    }
}