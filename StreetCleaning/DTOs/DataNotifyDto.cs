namespace StreetCleaning.DTOs
{
    public class DataNotifyDto
    {
        public DateTime PlanStart { get; set; }
        public DateTime PlanEnd { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public DateTime? Cancelled { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Finished { get; set; }
        public DateTime Published { get; set; }
    }
}
