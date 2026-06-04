using Microsoft.AspNetCore.Mvc.ModelBinding;
using StreetCleaning.Enums;
using StreetCleaning.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetCleaningTests.ViewModels
{
    public class IndexFormViewModelTests
    {
        private IndexFormViewModel _indexFormViewModel;

        public IndexFormViewModelTests()
        {
            _indexFormViewModel = new IndexFormViewModel
            {
                From = DateTime.Today,
                To = DateTime.Today,
                Type = NotifyType.Planned,
                City = "Žilina" // valid city to pass the filter requirement
            };
        }

        [Fact]
        public void TryValidateTo_NoFiltersFilled_AddsModelLevelErrorAndReturnsFalse()
        {
            _indexFormViewModel.City = null; // Remove the valid city to ensure no filters are filled
            var ms = new ModelStateDictionary();

            var ok = _indexFormViewModel.TryValidateTo(ms);
            List<string> msgs = ms.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            Assert.False(ok);
            Assert.False(ms.IsValid);
            Assert.NotEmpty(msgs);
            Assert.Contains("Musí byť uvedená aspoň jedna z položiek: EIC, Číslo odber. miesta, Obec, Ulica.", msgs);
        }

        [Fact]
        public void TryValidateTo_ValidModel_ReturnsTrue_AndNoErrors()
        {
            var ms = new ModelStateDictionary();

            var ok = _indexFormViewModel.TryValidateTo(ms);

            Assert.True(ok);
            Assert.True(ms.IsValid);
        }

        [Fact]
        public void TryValidateTo_FromGreaterThanTo_AddsErrorForFromAndTo()
        {
            _indexFormViewModel.From = DateTime.Today.AddDays(1);
            _indexFormViewModel.To = DateTime.Today;

            var ms = new ModelStateDictionary();

            var ok = _indexFormViewModel.TryValidateTo(ms);
            List<string> msgs = ms.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            Assert.False(ok);
            Assert.False(ms.IsValid);
            Assert.NotEmpty(msgs);
            Assert.Contains("Dátum 'Od' nemôže byť väčší ako 'Do'.", msgs);
        }

        [Fact]
        public void TryValidateTo_IntervalOver365Days_AddsErrorForFromAndTo()
        {
            _indexFormViewModel.To = _indexFormViewModel.To.AddDays(366);
            var ms = new ModelStateDictionary();

            var ok = _indexFormViewModel.TryValidateTo(ms);
            List<string> msgs = ms.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            Assert.False(ok);
            Assert.False(ms.IsValid);
            Assert.NotEmpty(msgs);
            Assert.Contains("Maximálny povolený interval je 1 rok.", msgs);
        }

        [Theory]
        [InlineData("short")]                       // <16
        [InlineData("ABCDEFGHIJKLMNOPQ")]           // >16
        [InlineData("ABCD-1234-5678-90")]           // non-alnum
        public void TryValidateTo_InvalidEic_AddsErrorForEic(string eic)
        {
            _indexFormViewModel.Eic = eic;
            var ms = new ModelStateDictionary();

            var ok = _indexFormViewModel.TryValidateTo(ms);
            List<string> msgs = ms.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            Assert.False(ok);
            Assert.False(ms.IsValid);
            var entry = Assert.Contains("Filter.Eic", ms);
            Assert.NotEmpty(msgs);
            Assert.Contains("EIC kód musí mať presne 16 znakov (iba písmená a čísla).", msgs);
        }

        [Fact]
        public void TryValidateTo_EicIsUppercasedByNormalize()
        {
            _indexFormViewModel.Eic = "abcd1234efgh5678";
            var ms = new ModelStateDictionary();

            var _ = _indexFormViewModel.TryValidateTo(ms);

            Assert.Equal("ABCD1234EFGH5678", _indexFormViewModel.Eic);
        }

        [Theory]
        [InlineData("1234567")]     // 7 digits
        [InlineData("123456789")]   // 9 digits
        [InlineData("12A45678")]    // non-digit
        public void TryValidateTo_InvalidConsumptionPointNumber_AddsError(string cpn)
        {
            _indexFormViewModel.ConsumptionPointNumber = cpn;
            var ms = new ModelStateDictionary();

            var ok = _indexFormViewModel.TryValidateTo(ms);
            List<string> msgs = ms.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            Assert.False(ok);
            Assert.False(ms.IsValid);
            Assert.NotEmpty(msgs);
            Assert.Contains("Číslo odberného miesta musí obsahovať presne 8 číslic.", msgs);
        }

        [Fact]
        public void TryValidateTo_CityTooShort_AddsDataAnnotationsError()
        {
            _indexFormViewModel.City = "Ži";
            var ms = new ModelStateDictionary();

            var ok = _indexFormViewModel.TryValidateTo(ms);
            List<string> msgs = ms.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            Assert.False(ok);
            var entry = Assert.Contains("Filter.City", ms);
            Assert.NotEmpty(msgs);
            Assert.Contains("Názov obce musí obsahovať aspoň 3 znaky!", msgs);
        }

        [Fact]
        public void TryValidateTo_CityTooLong_AddsDataAnnotationsError()
        {
            _indexFormViewModel.City = "Žiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii";
            var ms = new ModelStateDictionary();

            var ok = _indexFormViewModel.TryValidateTo(ms);
            List<string> msgs = ms.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            Assert.False(ok);
            var entry = Assert.Contains("Filter.City", ms);
            Assert.NotEmpty(msgs);
            Assert.Contains("Obec nesmie byť dlhšia ako 30 znakov.", msgs);
        }

        [Fact]
        public void TryValidateTo_CityHasForbiddenChars_AddsCustomError()
        {
            _indexFormViewModel.City = "Žilina;";
            var ms = new ModelStateDictionary();

            var ok = _indexFormViewModel.TryValidateTo(ms);
            List<string> msgs = ms.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            Assert.False(ok);
            var entry = Assert.Contains("Filter.City", ms);
            Assert.NotEmpty(msgs);
            Assert.Contains("Obec obsahuje nepovolené znaky.", msgs);
        }

        [Fact]
        public void TryValidateTo_StreetTooShort_AddsDataAnnotationsError()
        {
            _indexFormViewModel.Street = "Hl";
            var ms = new ModelStateDictionary();

            var ok = _indexFormViewModel.TryValidateTo(ms);
            List<string> msgs = ms.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            Assert.False(ok);
            var entry = Assert.Contains("Filter.Street", ms);
            Assert.NotEmpty(msgs);
            Assert.Contains("Názov ulice musí obsahovať aspoň 3 znaky!", msgs);
        }

        [Fact]
        public void TryValidateTo_StreetTooLong_AddsDataAnnotationsError()
        {
            _indexFormViewModel.Street = "Hlaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            var ms = new ModelStateDictionary();

            var ok = _indexFormViewModel.TryValidateTo(ms);
            List<string> msgs = ms.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            Assert.False(ok);
            var entry = Assert.Contains("Filter.Street", ms);
            Assert.NotEmpty(msgs);
            Assert.Contains("Ulica nesmie byť dlhšia ako 30 znakov.", msgs);
        }

        [Fact]
        public void TryValidateTo_StreetHasForbiddenChars_AddsCustomError()
        {
            _indexFormViewModel.Street = "Hlavná;";
            var ms = new ModelStateDictionary();

            var ok = _indexFormViewModel.TryValidateTo(ms);
            List<string> msgs = ms.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            Assert.False(ok);
            var entry = Assert.Contains("Filter.Street", ms);
            Assert.NotEmpty(msgs);
            Assert.Contains("Ulica obsahuje nepovolené znaky.", msgs);
        }

        [Fact]
        public void IsDefaultPlannedToday_True_WhenUnfilteredTodayPlanned()
        {
            _indexFormViewModel.City = null;

            Assert.True(_indexFormViewModel.IsDefaultPlannedToday());
        }

        [Fact]
        public void IsDefaultPlannedToday_False_WhenAnyFilterFilled()
        {
            Assert.False(_indexFormViewModel.IsDefaultPlannedToday());
        }
    }
}
