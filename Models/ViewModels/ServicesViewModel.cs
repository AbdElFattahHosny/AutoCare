using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models.ViewModels
{
    public class ServicesViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public double price { get; set; }
        [Required]
        public long priceInPoints { get; set; }
        [Required]
        public int Earnedpoints { get; set; }
    }
}
