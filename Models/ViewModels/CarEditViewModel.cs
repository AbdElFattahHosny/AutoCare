using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models.ViewModels
{
    public class CarEditViewModel
    {
      
        [Required]
        [Range(0, 999, ErrorMessage = "Enter Just 3 numbers")]
        public int CarNumbers { get; set; }
        [Required]
        public string Type { get; set; }
        [Range(1900, 2022, ErrorMessage = "Enter valid Model Year")]
        public int Model { get; set; }
        [Display(Name = "owner")]
        public string ownerId { get; set; }

        public IEnumerable<ApplicationUser> owners { get; set; }

     
    }
}
