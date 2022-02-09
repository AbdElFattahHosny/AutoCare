using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models
{
    public class ApplicationUser : IdentityUser
    {

        [Required]

        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Key]
        [Required]
        [Display(Name = "Nationality Num")]
        [RegularExpression(@"^([0-9]{7})$", ErrorMessage = "Invalid ID Number.")]
        public long NationalNumber { get; set; }
      
        public string Phone { get; set; }
       
        [Required]
        public string Address { get; set; }

        public string AddedDate { get; set; }
        
        public string EditedDate { get; set; }
        public ICollection<Cars> cars { get; set; }


    }
}
