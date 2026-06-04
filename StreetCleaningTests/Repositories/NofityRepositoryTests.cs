using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StreetCleaning.Data;
using StreetCleaning.Enums;
using StreetCleaning.Models;
using StreetCleaning.Repositories;
using StreetCleaning.ViewModels;
using StreetCleaningTests.TestModifyModels;

namespace StreetCleaningTests.Repositories
{
    public class NofityRepositoryTests : IDisposable
    {
        private readonly ModelDbContext _dbContext;
        private readonly NotifyRepository _repository;
        private readonly SqliteConnection _connection;

        public NofityRepositoryTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<ModelDbContext>()
                .UseSqlite(_connection)
                .Options;
            IConfiguration cfg = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Oracle:PlnOdsObject", "PLN_ODS_NOTIFY" }
                })
                .Build();
            _dbContext = new TestDbContext(options, cfg);
            _dbContext.Database.EnsureCreated();

            _repository = new NotifyRepository(_dbContext);

            Seed();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _connection.Dispose();
        }

        private void Seed()
        {
            _dbContext.DataNotifies.AddRange(
                new DataNotify
                {
                    Eic = "24ZSS73237160001",
                    Com = 1234567,
                    Obec = "Lysá pod Makytou",
                    Ulica = "Lysá pod Makytou",
                    CDomu = "229",
                    PlanOd = new DateTime(2025, 9, 1, 8, 0, 0),
                    PlanDo = new DateTime(2025, 9, 1, 12, 0, 0),
                    Zahajene = new DateTime(2025, 9, 1, 8, 0, 0),
                    Ukoncene = new DateTime(2025, 9, 1, 12, 0, 0),
                    Vlozene = new DateTime(2025, 8, 15, 9, 0, 0)
                },

                new DataNotify
                {
                    Eic = "24ZSS73237160000",
                    Com = 1234588,
                    Obec = "Lysá pod Makytou",
                    Ulica = "Lysá pod Makytou",
                    CDomu = "229",
                    PlanOd = new DateTime(2025, 9, 1, 8, 0, 0),
                    PlanDo = new DateTime(2025, 9, 1, 12, 0, 0),
                    Zahajene = new DateTime(2025, 9, 1, 8, 0, 0),
                    Ukoncene = new DateTime(2025, 9, 1, 12, 0, 0),
                    Vlozene = new DateTime(2025, 8, 15, 9, 0, 0)
                },

                new DataNotify
                {
                    Eic = "24ZSS73237160002",
                    Com = 1234568,
                    Obec = "Lysá pod Makytou",
                    Ulica = "Lysá pod Makytou",
                    CDomu = "381",
                    PlanOd = new DateTime(2025, 9, 1, 8, 0, 0),
                    PlanDo = new DateTime(2025, 9, 1, 12, 0, 0),
                    Zahajene = new DateTime(2025, 9, 1, 8, 0, 0),
                    Ukoncene = new DateTime(2025, 9, 1, 12, 0, 0),
                    Vlozene = new DateTime(2025, 8, 15, 9, 0, 0)
                },

                new DataNotify
                {
                    Eic = "24ZSS73237160002",
                    Com = 1234568,
                    Obec = "Lysá pod Makytou",
                    Ulica = "Lysá pod Makytou",
                    CDomu = "381",
                    PlanOd = new DateTime(2025, 7, 1, 8, 0, 0),
                    PlanDo = new DateTime(2025, 7, 6, 12, 0, 0),
                    Zahajene = new DateTime(2025, 7, 1, 8, 0, 0),
                    Ukoncene = new DateTime(2025, 7, 6, 12, 0, 0),
                    Vlozene = new DateTime(2025, 8, 15, 9, 0, 0)
                },

                new DataNotify
                {
                    Eic = "24ZSS73237160003",
                    Com = 1234569,
                    Obec = "Ľuboriečka",
                    Ulica = "Ľuboriečka",
                    CDomu = "75",
                    PlanOd = new DateTime(2025, 9, 1, 8, 0, 0),
                    PlanDo = new DateTime(2025, 9, 1, 12, 0, 0),
                    Zahajene = new DateTime(2025, 9, 1, 8, 0, 0),
                    Ukoncene = new DateTime(2025, 9, 1, 12, 0, 0),
                    Vlozene = new DateTime(2025, 8, 15, 9, 0, 0)
                },

                new DataNotify
                {
                    Eic = "24ZSS73237160003",
                    Com = 1234569,
                    Obec = "Ľuboriečka",
                    Ulica = "Ľuboriečka",
                    CDomu = "75",
                    PlanOd = new DateTime(2025, 6, 8, 8, 0, 0),
                    PlanDo = new DateTime(2025, 7, 9, 12, 0, 0),
                    Zahajene = new DateTime(2025, 6, 8, 8, 0, 0),
                    Ukoncene = new DateTime(2025, 7, 9, 12, 0, 0),
                    Vlozene = new DateTime(2025, 8, 15, 9, 0, 0)
                },

                new DataNotify
                {
                    Eic = "24ZSS73237160004",
                    Com = 1234570,
                    Obec = "Bobrov",
                    Ulica = "Za školou",
                    CDomu = "681",
                    PlanOd = new DateTime(2025, 9, 1, 8, 0, 0),
                    PlanDo = new DateTime(2025, 9, 1, 12, 0, 0),
                    Zahajene = new DateTime(2025, 9, 1, 8, 0, 0),
                    Ukoncene = new DateTime(2025, 9, 1, 12, 0, 0),
                    Vlozene = new DateTime(2025, 8, 15, 9, 0, 0)
                },

                new DataNotify
                {
                    Eic = "24ZSS73237160004",
                    Com = 1234570,
                    Obec = "Bobrov",
                    Ulica = "Za školou",
                    CDomu = "682",
                    PlanOd = new DateTime(2025, 9, 1, 8, 0, 0),
                    PlanDo = new DateTime(2025, 9, 1, 12, 0, 0),
                    Zahajene = new DateTime(2025, 9, 1, 8, 0, 0),
                    Ukoncene = new DateTime(2025, 9, 1, 12, 0, 0),
                    Vlozene = new DateTime(2025, 8, 15, 9, 0, 0)
                },

                new DataNotify
                {
                    Eic = "24ZSS73237160005",
                    Com = 1234571,
                    Obec = "Hronská Dúbrava",
                    Ulica = "Dolná",
                    CDomu = "73",
                    PlanOd = new DateTime(2025, 9, 1, 8, 0, 0),
                    PlanDo = new DateTime(2025, 9, 1, 12, 0, 0),
                    Stornovane = new DateTime(2025, 8, 1, 12, 0, 0),
                    Vlozene = new DateTime(2025, 8, 15, 9, 0, 0)
                },

                new DataNotify
                {
                    Eic = "24ZSS73237160005",
                    Com = 1234571,
                    Obec = "Hronská Dúbrava",
                    Ulica = "Dolná",
                    CDomu = "74",
                    PlanOd = new DateTime(2025, 9, 1, 8, 0, 0),
                    PlanDo = new DateTime(2025, 9, 1, 12, 0, 0),
                    Stornovane = new DateTime(2025, 8, 1, 12, 0, 0),
                    Vlozene = new DateTime(2025, 8, 15, 9, 0, 0)
                }
            );

            _dbContext.SaveChanges();
        }

        [Fact]
        public void Search_FilterByCity()
        {
            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 9, 1, 8, 0, 0),
                To = new DateTime(2025, 12, 30, 12, 0, 0),
                Type = NotifyType.Planned,
                City = "Lysá pod Makytou"
            };

            var list = _repository.SearchByFilter(filter);

            Assert.NotEmpty(list);
            Assert.All(list, item => Assert.Equal("Lysá pod Makytou", item.City));
        }

        [Fact]
        public void Search_FilterByCity_Deduplicates()
        {
            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 9, 1, 8, 0, 0),
                To = new DateTime(2025, 12, 30, 12, 0, 0),
                Type = NotifyType.Planned,
                City = "Lysá pod Makytou"
            };

            var list = _repository.SearchByFilter(filter);

            //we seed 4 rows, first two collapse to one after group by
            Assert.NotEmpty(list);
            Assert.Equal(3, list.Count);
        }

        [Fact]
        public void Search_FilterByCity_NotFullInformation()
        {
            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 9, 1, 8, 0, 0),
                To = new DateTime(2025, 12, 30, 12, 0, 0),
                Type = NotifyType.Planned,
                City = "Lysá pod"
            };

            var list = _repository.SearchByFilter(filter);

            Assert.NotEmpty(list);
            Assert.All(list, item => Assert.Equal("Lysá pod Makytou", item.City));
        }

        [Fact]
        public void Search_FilterByEic()
        {
            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 9, 1, 8, 0, 0),
                To = new DateTime(2025, 12, 30, 12, 0, 0),
                Type = NotifyType.Planned,
                Eic = "24ZSS73237160002"
            };

            var list = _repository.SearchByFilter(filter);

            Assert.NotEmpty(list);
            Assert.All(list, item => Assert.Equal("Lysá pod Makytou", item.City));
            Assert.All(list, item => Assert.Equal("Lysá pod Makytou", item.Street));
            Assert.All(list, item => Assert.Equal("381", item.HouseNumber));
        }

        [Fact]
        public void Search_FilterByConsumptionPointNumber()
        {
            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 9, 1, 8, 0, 0),
                To = new DateTime(2025, 12, 30, 12, 0, 0),
                Type = NotifyType.Planned,
                ConsumptionPointNumber = "1234569"
            };

            var list = _repository.SearchByFilter(filter);

            Assert.NotEmpty(list);
            Assert.All(list, item => Assert.Equal("Ľuboriečka", item.City));
            Assert.All(list, item => Assert.Equal("Ľuboriečka", item.Street));
            Assert.All(list, item => Assert.Equal("75", item.HouseNumber));
        }

        [Fact]
        public void Search_FilterByStreet()
        {
            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 9, 1, 8, 0, 0),
                To = new DateTime(2025, 12, 30, 12, 0, 0),
                Type = NotifyType.Planned,
                Street = "Za školou"
            };

            var list = _repository.SearchByFilter(filter);

            Assert.NotEmpty(list);
            Assert.All(list, item => Assert.Equal("Bobrov", item.City));
            Assert.All(list, item => Assert.Equal("Za školou", item.Street));
        }

        [Fact]
        public void Search_FilterByStreet_NotFullInformation()
        {
            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 9, 1, 8, 0, 0),
                To = new DateTime(2025, 12, 30, 12, 0, 0),
                Type = NotifyType.Planned,
                Street = "Za ško"
            };

            var list = _repository.SearchByFilter(filter);

            Assert.NotEmpty(list);
            Assert.All(list, item => Assert.Equal("Bobrov", item.City));
            Assert.All(list, item => Assert.Equal("Za školou", item.Street));
        }

        [Fact]
        public void Search_FilterByStreetAndNumber()
        {
            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 9, 1, 8, 0, 0),
                To = new DateTime(2025, 12, 30, 12, 0, 0),
                Type = NotifyType.Planned,
                Street = "Za školou 681"
            };

            var list = _repository.SearchByFilter(filter);

            Assert.NotEmpty(list);
            Assert.All(list, item => Assert.Equal("Bobrov", item.City));
            Assert.All(list, item => Assert.Equal("Za školou", item.Street));
            Assert.All(list, item => Assert.Equal("681", item.HouseNumber));
        }

        [Fact]
        public void Search_FilterByStornoAndCity()
        {
            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 9, 1, 8, 0, 0),
                To = new DateTime(2025, 12, 30, 12, 0, 0),
                Type = NotifyType.Cancelled,
                City = "Hronská Dúbrava"
            };

            var list = _repository.SearchByFilter(filter);

            Assert.NotEmpty(list);
            Assert.All(list, item => Assert.Equal("Hronská Dúbrava", item.City));
            Assert.All(list, item => Assert.Equal("Dolná", item.Street));
            Assert.All(list, item => Assert.NotNull(item.Cancelled));
        }

        [Fact]
        public void Search_FilterByStornoAndStreet()
        {
            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 9, 1, 8, 0, 0),
                To = new DateTime(2025, 12, 30, 12, 0, 0),
                Type = NotifyType.Cancelled,
                Street = "Dolná"
            };

            var list = _repository.SearchByFilter(filter);

            Assert.NotEmpty(list);
            Assert.All(list, item => Assert.Equal("Hronská Dúbrava", item.City));
            Assert.All(list, item => Assert.Equal("Dolná", item.Street));
            Assert.All(list, item => Assert.NotNull(item.Cancelled));
        }

        [Fact]
        public void Search_FilterByFinishedAndCity()
        {
            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 9, 1, 8, 0, 0),
                To = new DateTime(2025, 12, 30, 12, 0, 0),
                Type = NotifyType.Actual,
                City = "Bobrov"
            };

            var list = _repository.SearchByFilter(filter);

            Assert.NotEmpty(list);
            Assert.All(list, item => Assert.Equal("Bobrov", item.City));
            Assert.All(list, item => Assert.Equal("Za školou", item.Street));
            Assert.All(list, item => Assert.NotNull(item.Started));
            Assert.All(list, item => Assert.NotNull(item.Finished));
        }

        [Fact]
        public void Search_FilterByFinishedAndStreet()
        {
            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 9, 1, 8, 0, 0),
                To = new DateTime(2025, 12, 30, 12, 0, 0),
                Type = NotifyType.Actual,
                Street = "Za škol"
            };

            var list = _repository.SearchByFilter(filter);

            Assert.NotEmpty(list);
            Assert.All(list, item => Assert.Equal("Bobrov", item.City));
            Assert.All(list, item => Assert.Equal("Za školou", item.Street));
            Assert.All(list, item => Assert.NotNull(item.Started));
            Assert.All(list, item => Assert.NotNull(item.Finished));
        }

        [Fact]
        public void Search_Order_NullsInHouseNumberLast()
        {
            _dbContext.DataNotifies.Add(new DataNotify
            {
                Eic = "24ZSS73237169999",
                Com = 5555555,
                Obec = "Bobrov",
                Ulica = "Za školou",
                CDomu = null, // <— important
                PlanOd = new DateTime(2025, 9, 1, 8, 0, 0),
                PlanDo = new DateTime(2025, 9, 1, 12, 0, 0),
                Zahajene = new DateTime(2025, 9, 1, 8, 0, 0),
                Ukoncene = new DateTime(2025, 9, 1, 12, 0, 0),
                Vlozene = new DateTime(2025, 8, 15, 9, 0, 0)
            });
            _dbContext.SaveChanges();

            var filter = new IndexFormViewModel
            {
                From = new DateTime(2025, 9, 1),
                To = new DateTime(2025, 9, 1),
                Type = NotifyType.Planned,
                City = "Bobrov"
            };

            var list = _repository.SearchByFilter(filter);

            Assert.NotEmpty(list);
            // last item should have null HouseNumber
            Assert.Null(list.Last().HouseNumber);
        }

        [Fact]
        public void Search_Paging_NoMoreResults_Toggles()
        {
            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 1, 1),
                To = new DateTime(2025, 12, 31),
                Type = NotifyType.Planned,
                City = "Lysá pod Makytou",
                SelectedAmount = 2,
                SkipAmount = 0
            };

            var list = _repository.SearchByFilter(filter);

            Assert.True(list.Count <= 3);
            Assert.Equal("false", filter.NoMoreResults);

            filter.SkipAmount = 2;
            list = _repository.SearchByFilter(filter);

            Assert.True(list.Count <= 2);
            Assert.Equal("true", filter.NoMoreResults);
        }

        [Fact]
        public void Search_Street_Contains_MiddleSubstring()
        {
            var filter = new IndexFormViewModel
            {
                From = new DateTime(2024, 9, 1),
                To = new DateTime(2025, 12, 31),
                Type = NotifyType.Planned,
                Street = " škol"
            };

            var list = _repository.SearchByFilter(filter);

            Assert.NotEmpty(list);
            Assert.All(list, x => Assert.Equal("Za školou", x.Street));
        }
    }
}
