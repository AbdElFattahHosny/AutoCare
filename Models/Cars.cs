using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCare.Models
{
    public class Cars
    {
        [Key]
        [Column(TypeName = "bigint")]
        public long ID { get; set; }
        [Required]
        [Range(0,999)]
        public int CarNumbers { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int Model { get; set; }

        //public ICollection<checkUp> CheckUps { get; set; }

        public string OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public ApplicationUser applicationUser { get; set; }
        public string AddedDate { get; set; }

        public string EditedDate { get; set; }

    }
}
