using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoCare.Models.ViewModels
{
    public class CheckUpSpareServicesViewModel
    {
        [Required]
        [Display(Name = "Car Counter")]
        public long Counter { get; set; }

        [Display(Name = "Car Numbers")]
        public long CarId { get; set; }

        //[Required]
        //public List<int> Quantity { get; set; }
        public IEnumerable<Cars> cars { get; set; }

        public List<CheckBoxServices> Services { get; set; }

        public List<CheckBoxSpareParts> SpareParts { get; set; }
    }
}
