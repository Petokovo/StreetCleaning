using StreetCleaning.DTOs;
using StreetCleaning.ViewModels;

namespace StreetCleaning.Repositories
{
    public interface INotifyRepository
    {
        /// <summary>
        /// Searches planned cleaning records by the given filter:
        /// - Date window by Type (Planned/Cancelled/Actual).
        /// - Optional filters: EIC, ConsumptionPointNumber, City, Street.
        /// - Projects to DTO, de-duplicates, sets NoMoreResults, orders and limits.
        /// </summary>
        /// <param name="formViewModel">
        /// User filter: From/To dates, Type, EIC, ConsumptionPointNumber, City, Street,
        /// paging fields (SelectedAmount/NoMoreResults).
        /// </param>
        List<DataNotifyDto> SearchByFilter(IndexFormViewModel notifyFormViewModel);

    }
}
