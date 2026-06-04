using Microsoft.AspNetCore.Mvc.ModelBinding;
using StreetCleaning.Enums;
using StreetCleaning.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace StreetCleaning.ViewModels
{
    public class IndexFormViewModel : Pager
    {
        public IndexFormViewModel() : base() { }

        [DataType(DataType.Date, ErrorMessage = "Zlý formát")]
        [Display(Name = "Od")]
        [Required(ErrorMessage = "Pole Od je povinné.")]
        public DateTime From { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Zlý formát")]
        [Display(Name = "Do")]
        [Required(ErrorMessage = "Pole Do je povinné.")]
        public DateTime To { get; set; }

        [Display(Name = "Typ")]
        public required NotifyType Type { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "EIC kód")]
        [RegularExpression(@"^[A-Za-z0-9]{16}$", ErrorMessage = "EIC kód musí mať presne 16 znakov (iba písmená a čísla).")]
        public string? Eic { get; set; }

        [Display(Name = "Číslo odber. miesta")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Číslo odberného miesta musí obsahovať presne 8 číslic.")]
        public string? ConsumptionPointNumber { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Obec")]
        [MinLength(3, ErrorMessage = "Názov obce musí obsahovať aspoň 3 znaky!")]
        [MaxLength(30, ErrorMessage = "Obec nesmie byť dlhšia ako 30 znakov.")]
        public string? City { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Ulica")]
        [MinLength(3, ErrorMessage = "Názov ulice musí obsahovať aspoň 3 znaky!")]
        [MaxLength(30, ErrorMessage = "Ulica nesmie byť dlhšia ako 30 znakov.")]
        public string? Street { get; set; }

        private static readonly List<char> BlackList = new()
        {
            '<','>','"','\'','`','&',';','%','_','@',
            '\\','|','/','^','~','[',']','{','}','(',')','$','#'
        };

        /// <summary>
        /// Checks if the given string contains any forbidden characters.
        /// </summary>
        /// <param name="input">The string to validate.</param>
        /// <returns>
        /// True if the input contains at least one character from the BlackList,
        /// otherwise false.
        /// </returns>
        private bool ContainsForbiddenChars(string input)
        {
            foreach (char ch in input)
            {
                if (BlackList.Contains(ch))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the form is in default state (From/To = today, Type = Planned, no filters).
        /// </summary>
        /// <returns>
        /// True if default, otherwise false.
        /// </returns>
        public bool IsDefaultPlannedToday()
        {
            return From == DateTime.Today && To == DateTime.Today && Type == NotifyType.Planned && string.IsNullOrEmpty(Eic) &&
            string.IsNullOrEmpty(ConsumptionPointNumber) && string.IsNullOrEmpty(City) && string.IsNullOrEmpty(Street);
        }

        /// <summary>
        /// Checks if at least one filter field is filled (EIC, ConsumptionPointNumber, City, Street).
        /// </summary>
        /// <returns>
        /// True if at least one is non-empty, otherwise false.
        /// </returns>
        private bool AnyFilterFilled()
        {
            return !string.IsNullOrWhiteSpace(Eic) || !string.IsNullOrWhiteSpace(ConsumptionPointNumber) ||
                   !string.IsNullOrWhiteSpace(City) || !string.IsNullOrWhiteSpace(Street);
        }

        /// <summary>
        /// Trims and normalizes input fields (EIC → lowercase).
        /// </summary>
        /// <returns>
        /// Nothing (void). Updates the model in place.
        /// </returns
        private void Normalize()
        {
            Eic = Eic?.Trim().ToUpper();
            ConsumptionPointNumber = ConsumptionPointNumber?.Trim();
            City = City?.Trim();
            Street = Street?.Trim();
        }

        /// <summary>
        /// Runs DataAnnotations and custom validation rules for the form.
        /// </summary>
        /// <returns>
        /// IEnumerable of ValidationResult with all validation errors, or empty if valid.
        /// </returns>
        private IEnumerable<ValidationResult> ValidateAll()
        {
            Normalize();

            var results = new List<ValidationResult>();
            var ctx = new ValidationContext(this);
            Validator.TryValidateObject(
                instance: this,
                validationContext: ctx,
                validationResults: results,
                validateAllProperties: true
            );

            foreach (var r in results)
                yield return r;

            if (From.Date > To.Date)
                yield return new ValidationResult("Dátum 'Od' nemôže byť väčší ako 'Do'.", new[] { nameof(From), nameof(To) });

            if ((To.Date - From.Date).TotalDays > 365)
                yield return new ValidationResult("Maximálny povolený interval je 1 rok.", new[] { nameof(From), nameof(To) });

            if (!string.IsNullOrWhiteSpace(Eic) && !Regex.IsMatch(Eic, @"^[A-Za-z0-9]{16}$"))
                yield return new ValidationResult("EIC kód musí mať presne 16 znakov (iba písmená a čísla).", new[] { nameof(Eic) });

            if (!string.IsNullOrWhiteSpace(ConsumptionPointNumber) && !Regex.IsMatch(ConsumptionPointNumber, @"^\d{8}$"))
                yield return new ValidationResult("Číslo odberného miesta musí obsahovať presne 8 číslic.", new[] { nameof(ConsumptionPointNumber) });

            if (!string.IsNullOrWhiteSpace(City) && ContainsForbiddenChars(City))
                yield return new ValidationResult("Obec obsahuje nepovolené znaky.", new[] { nameof(City) });

            if (!string.IsNullOrWhiteSpace(Street) && ContainsForbiddenChars(Street))
                yield return new ValidationResult("Ulica obsahuje nepovolené znaky.", new[] { nameof(Street) });

            if (!AnyFilterFilled())
                yield return new ValidationResult(
                    "Musí byť uvedená aspoň jedna z položiek: EIC, Číslo odber. miesta, Obec, Ulica.");
        }

        /// <summary>
        /// Runs full validation and writes results into the given ModelState.
        /// </summary>
        /// <param name="modelState">
        /// Target ModelStateDictionary to record errors.
        /// </param>
        /// <returns>
        /// True if no errors, false if any validation errors were found.
        /// </returns>
        public bool TryValidateTo(ModelStateDictionary modelState)
        {
            var ok = true;
            foreach (var vr in ValidateAll())
            {
                var key = vr.MemberNames.FirstOrDefault() ?? string.Empty;
                if (string.IsNullOrEmpty(key))
                    modelState.AddModelError(string.Empty, vr.ErrorMessage ?? "");
                else
                    modelState.AddModelError("Filter." + key, vr.ErrorMessage ?? "");
                ok = false;
            }
            return ok;
        }
    }
}
