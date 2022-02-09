using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models
{
    public class services
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public double price { get; set; }
        [Required]
        public long priceInPoints { get; set; }
        [Required]
        public int Earnedpoints { get; set; }

        public ICollection<ServicesCheckups> checkupRelation { get; set; }

        //public ICollection<checkUp> checkUps { get; set; }
        public string AddedDate { get; set; }

        public string EditedDate { get; set; }

    }
}
