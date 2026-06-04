using StreetCleaning.ViewModels;

namespace StreetCleaning.Services.Interfaces
{
    public interface INotifyService
    {
        /// <summary>
        /// Filters planned cleannings records using the given form filter.
        /// </summary>
        /// <param name="nofityFormViewModel">
        /// Filter criteria from user input.
        /// </param>
        /// <returns>
        /// IndexPageViewModel containing the applied filter and results list (empty if no matches).
        /// </returns>
        public IndexPageViewModel FilterByNotifyFormViewModel(IndexFormViewModel nofityFormViewModel);
    }
}
