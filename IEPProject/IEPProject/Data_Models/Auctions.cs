using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEPProject.Data_Models
{
    public class Auction
    {
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
    }
}
