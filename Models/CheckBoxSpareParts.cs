using System.Collections.Generic;

namespace AutoCare.Models
{
    public class CheckBoxSpareParts
    {
        public long ID { get; set; }

        public string Details { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public bool IsChecked { get; set; }
    }
}
