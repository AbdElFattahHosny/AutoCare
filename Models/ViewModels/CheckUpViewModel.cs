namespace AutoCare.Models.ViewModels
{
    public class CheckUpViewModel
    {
        public long Counter { get; set; }
        public Cars car { get; set; }

        public ApplicationUser Customer { get; set; }
        
        public SpareParts spareParts { get; set; }

        public services services { get; set; }


    }
}
