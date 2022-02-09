using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoCare.Models
{
    public class checkUp
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Car Counter")]
        public long Counter { get; set; }
        [Required]
        [Display(Name = "Arrive Date")]
        public DateTime Date { get; set; }
        
        public double TotalCost { get; set; }
        
        public int TotalPoints { get; set; }
        [Display(Name = "Car Numbers")]
        public Cars car { get; set; }
        [Display(Name = "Car Numbers")]
        [ForeignKey(nameof(CarId))]
        [Required]
        public long CarId { get; set; }


        public ICollection<spareCheckup> spareRelation { get; set; }
        public ICollection<ServicesCheckups> servicesRelation { get; set; }


        //public ICollection<SpareParts> SpareParts { get; set; }

        //public ICollection<services> services { get; set; }


        public string EditedDate { get; set; }


    }
}
