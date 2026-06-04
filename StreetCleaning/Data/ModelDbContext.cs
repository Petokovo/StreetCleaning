using Microsoft.EntityFrameworkCore;
using StreetCleaning.Models;

namespace StreetCleaning.Data
{
    public partial class ModelDbContext : DbContext
    {
        private readonly string _notifyObject;

        public ModelDbContext(DbContextOptions<ModelDbContext> options, IConfiguration cfg)
            : base(options)
        {
            // Get the Oracle object name from configuration
            _notifyObject = cfg["Oracle:NotifyObject"] ?? "NOTIFY";
        }

        public virtual DbSet<DataNotify> DataNotifies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("USING_NLS_COMP");

            modelBuilder.Entity<DataNotify>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable(_notifyObject); // Use the configured Oracle object name

                entity.Property(e => e.CDomu)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("C_DOMU");
                entity.Property(e => e.Com)
                    .HasPrecision(8)
                    .HasColumnName("COM");
                entity.Property(e => e.Eic)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("EIC");
                entity.Property(e => e.Obec)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OBEC");
                entity.Property(e => e.PlanDo)
                    .HasColumnType("DATE")
                    .HasColumnName("PLAN_DO");
                entity.Property(e => e.PlanOd)
                    .HasColumnType("DATE")
                    .HasColumnName("PLAN_OD");
                entity.Property(e => e.Stornovane)
                    .HasColumnType("DATE")
                    .HasColumnName("STORNOVANE");
                entity.Property(e => e.Ukoncene)
                    .HasColumnType("DATE")
                    .HasColumnName("UKONCENE");
                entity.Property(e => e.Ulica)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ULICA");
                entity.Property(e => e.Vlozene)
                    .HasColumnType("DATE")
                    .HasColumnName("VLOZENE");
                entity.Property(e => e.Zahajene)
                    .HasColumnType("DATE")
                    .HasColumnName("ZAHAJENE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
