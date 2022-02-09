using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models
{
    public class spareCheckup
    {
        [Key]
        public long Id { get; set; }

        public SpareParts spareParts { get; set; }
        
       [ForeignKey("spareParts")]
        public long? spareId { get; set; }

        public checkUp check { get; set; }
        
        [ForeignKey("check")]
        public long? checkupId { get; set; }

        public int Quantity { get; set; }

    }
}
