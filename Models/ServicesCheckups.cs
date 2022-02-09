using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models
{
    public class ServicesCheckups
    {
        [Key]
        public long ID { get; set; }

        public services service { get; set; }
        [ForeignKey(nameof(serviceId))]
        public long? serviceId { get; set; }

        public checkUp checkUp { get; set; }
        [ForeignKey(nameof(checkupId))]
        public long? checkupId { get; set; }


    }
}
