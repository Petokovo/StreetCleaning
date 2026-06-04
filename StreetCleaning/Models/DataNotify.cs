namespace StreetCleaning.Models
{
    public class DataNotify
    {
        public string Eic { get; set; } = null!;
        /// <summary>
        /// Cislo odberneho miesta (COM)
        /// </summary>
        public int Com { get; set; }

        public string? Obec { get; set; }

        public string? Ulica { get; set; }

        public string? CDomu { get; set; }

        public DateTime PlanOd { get; set; }

        public DateTime PlanDo { get; set; }

        public DateTime? Stornovane { get; set; }

        public DateTime? Zahajene { get; set; }

        public DateTime? Ukoncene { get; set; }

        public DateTime Vlozene { get; set; }
    }
}
