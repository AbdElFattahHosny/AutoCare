using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<ApplicationUser>();
        }

        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        public List<ApplicationUser> Users { get; set; }
    }
}
