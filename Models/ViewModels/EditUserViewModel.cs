using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models.ViewModels
{
    public class EditUserViewModel
    {
        [Required]

        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Key]
        [Required]
        [Display(Name = "Last 7 numbers in ID")]
        [RegularExpression(@"^([0-9]{7})$", ErrorMessage = "Invalid ID Number.")]
        public long NationalNumber { get; set; }
        //[Required]
        //[RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        
        public string Email { get; set; }
        public string EditedDate { get; set; }
    }
}
