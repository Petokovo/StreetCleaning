using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StreetCleaning.Models;
using StreetCleaning.Services.Interfaces;
using StreetCleaning.ViewModels;
using System.Diagnostics;

namespace StreetCleaning.Controllers
{
    [EnableRateLimiting("PerIpBurstPolicy")]
    public class HomeController : Controller
    {
        private readonly INotifyService _notifyService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(INotifyService notifyService, ILogger<HomeController> logger)
        {
            _notifyService = notifyService;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index(IndexPageViewModel indexPageViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (indexPageViewModel.Filter.TryValidateTo(ModelState))
                    {
                        return View(_notifyService.FilterByNotifyFormViewModel(indexPageViewModel.Filter));
                    }

                    return View(new IndexPageViewModel { Filter = indexPageViewModel.Filter, Results = new List<IndexResultViewModel>() });
                }

                IndexFormViewModel indexFormViewModel = new IndexFormViewModel
                {
                    Type = Enums.NotifyType.Planned,
                    From = DateTime.Today,
                    To = DateTime.Today
                };
                return View(new IndexPageViewModel { Filter = indexFormViewModel, Results = new List<IndexResultViewModel>() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the Index action.");

                IndexFormViewModel indexFormViewModel = new IndexFormViewModel
                {
                    Type = Enums.NotifyType.Planned,
                    From = DateTime.Today,
                    To = DateTime.Today
                };
                return View(new IndexPageViewModel { Filter = indexFormViewModel, Results = new List<IndexResultViewModel>() });
            }
        }

        [HttpGet("/rows")]
        public IActionResult ResultsRows(IndexPageViewModel indexPageViewModel)
        {
            try
            {
                var ms = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();

                if (indexPageViewModel.Filter == null)
                {
                    return NoContent(); // 204
                }

                if (indexPageViewModel.Filter.TryValidateTo(ms))
                {
                    var result = _notifyService.FilterByNotifyFormViewModel(indexPageViewModel.Filter);

                    if (result.Results == null || result.Results.Count == 0)
                    {
                        return NoContent(); // 204
                    }

                    Response.Headers["NoMoreResults"] = result.Filter.NoMoreResults;

                    return PartialView("_ResultTable", result);
                }

                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the ResultsRows action.");
                return NoContent(); // 204
            }
        }
    }
}
