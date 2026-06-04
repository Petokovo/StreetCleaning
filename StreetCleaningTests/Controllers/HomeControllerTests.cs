using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StreetCleaning.Controllers;
using StreetCleaning.DTOs;
using StreetCleaning.Enums;
using StreetCleaning.Repositories;
using StreetCleaning.Services;
using StreetCleaning.ViewModels;

namespace StreetCleaningTests.Controllers
{
    public class HomeControllerTests
    {
        private HomeController _controller;

        public HomeControllerTests()
        {
            // Arrange
            var repo = new Mock<INotifyRepository>();

            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 9, 1, 8, 0, 0),
                To = new DateTime(2025, 12, 30, 12, 0, 0),
                Type = NotifyType.Planned,
                City = "Lysá pod Makytou"
            };

            repo.Setup(r => r.SearchByFilter(It.IsAny<IndexFormViewModel>())).Returns(new List<DataNotifyDto>
            {
                new DataNotifyDto
                {
                    City = "Lysá pod Makytou",
                    Street = "Lysá pod Makytou",
                    HouseNumber = "229",
                    PlanStart = new DateTime(2025, 9, 1, 8, 0, 0),
                    PlanEnd = new DateTime(2025, 9, 1, 12, 0, 0),
                    Started = new DateTime(2025, 9, 1, 8, 0, 0),
                    Finished = new DateTime(2025, 9, 1, 12, 0, 0),
                    Published = new DateTime(2025, 8, 15, 9, 0, 0)
                },

                new DataNotifyDto
                {
                    City = "Lysá pod Makytou",
                    Street = "Lysá pod Makytou",
                    HouseNumber = "381",
                    PlanStart = new DateTime(2025, 9, 1, 8, 0, 0),
                    PlanEnd = new DateTime(2025, 9, 1, 12, 0, 0),
                    Started = new DateTime(2025, 9, 1, 8, 0, 0),
                    Finished = new DateTime(2025, 9, 1, 12, 0, 0),
                    Published = new DateTime(2025, 8, 15, 9, 0, 0)
                }
            });

            var service = new NotifyService(repo.Object);
            var loggerMock = new Mock<ILogger<HomeController>>();

            _controller = new HomeController(service, loggerMock.Object);
        }

        [Fact]
        public void Index_WhenModelStateValid_ReturnsViewWithTwoResults()
        {
            var result = _controller.Index(new IndexPageViewModel
            {
                Filter = new IndexFormViewModel { From = DateTime.Today, To = DateTime.Today, Type = NotifyType.Planned, City = "Lysá pod Makytou" }
            });

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<IndexPageViewModel>(view.Model);

            if (model.Results == null)
            {
                Assert.Fail("Model.Results is null");
            }
            Assert.Equal(2, model.Results.Count);
        }

        [Fact]
        public void Index_WhenFilterInvalid_AddsModelError_AndReturnsEmptyResults()
        {
            var result = _controller.Index(new IndexPageViewModel
            {
                Filter = new IndexFormViewModel { From = DateTime.Today, To = DateTime.Today, Type = NotifyType.Planned }
            });

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<IndexPageViewModel>(view.Model);

            var msgs = _controller.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            Assert.False(_controller.ModelState.IsValid);
            Assert.Contains(msgs, m => m.Contains("Musí byť uvedená aspoň jedna z položiek: EIC, Číslo odber. miesta, Obec, Ulica."));
            if (model.Results == null)
            {
                Assert.Fail("Model.Results is null");
            }
            Assert.Empty(model.Results);
        }

        [Fact]
        public void Index_InvalidModelState_ReturnsNothing()
        {
            var input = new IndexPageViewModel
            {
                Filter = new IndexFormViewModel { From = DateTime.Today, To = DateTime.Today, Type = NotifyType.Planned, Eic = "489651" }
            };

            // Act
            var result = _controller.Index(input);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<IndexPageViewModel>(view.Model);

            var msgs = _controller.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            Assert.False(_controller.ModelState.IsValid);
            Assert.Contains(msgs, m => m.Contains("EIC kód musí mať presne 16 znakov (iba písmená a čísla)."));
            if (model.Results == null)
            {
                Assert.Fail("Model.Results is null");
            }
            Assert.Empty(model.Results);
        }
    }
}
