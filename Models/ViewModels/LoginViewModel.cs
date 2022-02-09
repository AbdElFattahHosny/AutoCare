using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]       
        public long NationalNumber { get; set; }

        [Required]
        [Display(Name = "Enter your password")]
        
        public string password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
