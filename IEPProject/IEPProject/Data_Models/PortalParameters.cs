using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IEPProject.Data_Models
{
    public class PortalParameters
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Rows of auctions per page")]
        public int N { get; set; }

        [Required]
        [Display(Name = "Default duration")]
        public int D { get; set; }

        [Required]
        [Display(Name = "Silver package")]
        public int S { get; set; }

        [Required]
        [Display(Name = "Gold package")]
        public int G { get; set; }

        [Required]
        [Display(Name = "Platinum package")]
        public int P { get; set; }
        
        [Required]
        [StringLength(100)]
        [Display(Name = "Currency")]
        public string C { get; set; }
    }
}