using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]

        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required]
        [Display(Name = "PhoneNumber")]
        public string password { get; set; }
        [Required]
        [Display(Name = "Last 7 numbers in ID")]
        [RegularExpression(@"^([0-9]{7})$", ErrorMessage = "Invalid ID Number.")]
        public long NationalNumber { get; set; }
        //[Required]
        //[RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public long PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid Email Address")]

        public string Email { get; set; }
    }
}
