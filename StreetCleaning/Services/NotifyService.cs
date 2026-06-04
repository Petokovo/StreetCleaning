using StreetCleaning.Repositories;
using StreetCleaning.Services.Interfaces;
using StreetCleaning.ViewModels;

namespace StreetCleaning.Services
{
    public class NotifyService : INotifyService
    {
        private readonly INotifyRepository _notifyRepository;

        public NotifyService(INotifyRepository notifyRepository)
        {
            _notifyRepository = notifyRepository;
        }


        public IndexPageViewModel FilterByNotifyFormViewModel(IndexFormViewModel notifyFormViewModel)
        {
            var list = _notifyRepository.SearchByFilter(notifyFormViewModel);

            if (list.Count > 0)
            {
                list = list.Take(notifyFormViewModel.SelectedAmount).ToList();

                List<IndexResultViewModel> result = [];

                foreach (var item in list)
                {
                    result.Add(new IndexResultViewModel
                    {
                        PlanStart = item.PlanStart,
                        PlanEnd = item.PlanEnd,
                        City = item.City,
                        Street = item.Street + " " + item.HouseNumber,
                        Cancelled = item.Cancelled,
                        Started = item.Started,
                        Finished = item.Finished,
                        Published = item.Published
                    });
                }

                return new IndexPageViewModel { Filter = notifyFormViewModel, Results = result };
            }
            return new IndexPageViewModel { Filter = notifyFormViewModel };
        }
    }
}
