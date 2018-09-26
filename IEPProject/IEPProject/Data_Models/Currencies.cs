using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IEPProject.Data_Models
{
    public class Currency
    {
        public Currency() { }

        public Currency(string name)
        {
            Name = name;
            Value = 1;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public double Value { get; set; }
    }
}