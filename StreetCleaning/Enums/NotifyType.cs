using System.ComponentModel.DataAnnotations;

namespace StreetCleaning.Enums
{
    public enum NotifyType
    {
        [Display(Name = "Plánované upratovanie")]
        Planned = 1,

        [Display(Name = "Storno plánovaného upratovania")]
        Cancelled = 2,

        [Display(Name = "Skutočné časy zrealizovaných plánovaných upratovaní")]
        Actual = 3
    }
}
