using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using StreetCleaning.Data;
using StreetCleaning.Models;


namespace StreetCleaningTests.TestModifyModels
{
    public class TestDbContext : ModelDbContext
    {
        public TestDbContext(DbContextOptions<ModelDbContext> options,
                    IConfiguration cfg) : base(options, cfg)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Pridáme tieňový PK len pre testy
            modelBuilder.Entity<DataNotify>()
                .Property<int>("__TestId")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DataNotify>()
                .HasKey("__TestId");
        }
    }
}
