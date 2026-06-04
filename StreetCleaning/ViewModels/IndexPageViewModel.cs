namespace StreetCleaning.ViewModels
{
    public class IndexPageViewModel
    {
        public required IndexFormViewModel Filter { get; set; }
        public List<IndexResultViewModel>? Results { get; set; }
    }
}
