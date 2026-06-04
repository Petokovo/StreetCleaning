namespace StreetCleaning.ViewModels
{
    public class IndexResultViewModel
    {
        public DateTime PlanStart { get; set; }
        public DateTime PlanEnd { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public DateTime? Cancelled { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Finished { get; set; }
        public DateTime Published { get; set; }
    }
}
