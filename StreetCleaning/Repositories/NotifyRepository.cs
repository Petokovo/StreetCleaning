using Microsoft.EntityFrameworkCore;
using StreetCleaning.Data;
using StreetCleaning.DTOs;
using StreetCleaning.Enums;
using StreetCleaning.Models;
using StreetCleaning.ViewModels;

namespace StreetCleaning.Repositories
{
    public class NotifyRepository : INotifyRepository
    {
        private readonly ModelDbContext _modelDbContext;

        public NotifyRepository(ModelDbContext modelDbContext)
        {
            _modelDbContext = modelDbContext;
        }

        public List<DataNotifyDto> SearchByFilter(IndexFormViewModel formViewModel)
        {
            // basic query without tracking, when we just read
            IQueryable<DataNotify> query = _modelDbContext.DataNotifies.AsNoTracking();

            var from = formViewModel.From.Date;
            var to = formViewModel.To.Date.AddDays(1);

            if (formViewModel.Type == NotifyType.Planned)
            {
                query = query.Where(t => t.PlanOd >= from && t.PlanDo < to);
            }
            else if (formViewModel.Type == NotifyType.Cancelled)
            {
                query = query.Where(t => t.Stornovane.HasValue && t.Stornovane >= from && t.Stornovane < to);
            }
            else
            {
                query = query.Where(t => t.Zahajene.HasValue && t.Ukoncene.HasValue && t.Zahajene >= from && t.Ukoncene < to);
            }

            // optional filter – only added when filled
            if (!string.IsNullOrWhiteSpace(formViewModel.Eic))
            {
                query = query.Where(t => t.Eic.ToUpper() == formViewModel.Eic);
            }

            // optional filter – only added when filled
            if (!string.IsNullOrWhiteSpace(formViewModel.ConsumptionPointNumber))
            {
                if (int.TryParse(formViewModel.ConsumptionPointNumber, out int comInt))
                {
                    query = query.Where(t => t.Com == comInt);
                }
            }

            // optional filter – only added when filled
            if (!string.IsNullOrWhiteSpace(formViewModel.City))
            {
                query = query.Where(t => t.Obec != null && EF.Functions.Like(t.Obec, formViewModel.City + "%"));
            }

            // optional filter – only added when filled
            if (!string.IsNullOrWhiteSpace(formViewModel.Street))
            {
                query = query.Where(t => EF.Functions.Like((t.Ulica ?? "") + " " + (t.CDomu ?? ""), "%" + formViewModel.Street + "%"));
            }

            var orderedQuery = query
                .GroupBy(t => new { t.Obec, t.Ulica, t.CDomu, t.PlanOd, t.PlanDo })
                .Select(g => new DataNotifyDto
                {
                    City = g.Key.Obec,
                    Street = g.Key.Ulica,
                    HouseNumber = g.Key.CDomu,
                    PlanStart = g.Key.PlanOd,
                    PlanEnd = g.Key.PlanDo,
                    Cancelled = g.Max(x => x.Stornovane),
                    Started = g.Max(x => x.Zahajene),
                    Finished = g.Max(x => x.Ukoncene),
                    Published = g.Max(x => x.Vlozene)
                })
                .OrderBy(x => x.City)
                .ThenBy(x => x.Street)
                .ThenBy(x => x.HouseNumber == null ? 1 : 0)
                .ThenBy(x => x.HouseNumber == null ? int.MaxValue : x.HouseNumber.Length)
                .ThenBy(x => x.HouseNumber)
                .Skip(formViewModel.SkipAmount)
                .Take(formViewModel.SelectedAmount + 1);

            var results = orderedQuery.ToList();

            if (results.Count <= formViewModel.SelectedAmount)
                formViewModel.NoMoreResults = "true";
            else
                formViewModel.NoMoreResults = "false";

            return results;
        }
    }
}
