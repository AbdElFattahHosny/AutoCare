using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models
{
    public class SpareParts
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public string Details { get; set; }
        [Required]
        public double Price { get; set; }

        public ICollection<spareCheckup> checkupRelation { get; set; }


        //public ICollection<checkUp> checkUps { get; set; }
        public string AddedDate { get; set; }

        public string EditedDate { get; set; }
    }
}
