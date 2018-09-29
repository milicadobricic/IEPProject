using IEPProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IEPProject.Data_Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Display (Name = "Number of tokens")]
        public int NumTokens { get; set; }

        [Required]
        [Display (Name = "State of order")]
        public OrderState State { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        [Display (Name = "Time of submittion")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime SubmittionTime { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Time of completion")]
        public DateTime? CompletionTime { get; set; }
    }
}