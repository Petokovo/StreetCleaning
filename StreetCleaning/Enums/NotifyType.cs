using System.ComponentModel.DataAnnotations;

namespace StreetCleaning.Enums
{
    public enum NotifyType//complete
    {
        [Display(Name = "Plánované odstávky")]
        Planned = 1,

        [Display(Name = "Storno plánovanej odstávky")]
        Cancelled = 2,

        [Display(Name = "Skutočné časy zrealizovaných plánovaných odstávok")]
        Actual = 3
    }
}
