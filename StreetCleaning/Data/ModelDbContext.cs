using Microsoft.EntityFrameworkCore;

namespace StreetCleaning.Data
{
    public partial class ModelDbContext : DbContext
    {
        private readonly string _plnOdsObject;

        public PlnOdsDbContext(DbContextOptions<PlnOdsDbContext> options, IConfiguration cfg)
            : base(options)
        {
            // Get the Oracle object name from configuration, default to "PLN_ODS_NOTIFY" if not set
            _plnOdsObject = cfg["Oracle:PlnOdsObject"] ?? "PLN_ODS_NOTIFY";
        }

        public virtual DbSet<PlnOdsNotify> PlnOdsNotifies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("USING_NLS_COMP");

            modelBuilder.Entity<PlnOdsNotify>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable(_plnOdsObject); // Use the configured Oracle object name

                entity.Property(e => e.CDomu)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("C_DOMU");
                entity.Property(e => e.CastObce)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CAST_OBCE");
                entity.Property(e => e.Com)
                    .HasPrecision(8)
                    .HasColumnName("COM");
                entity.Property(e => e.Eic)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("EIC");
                entity.Property(e => e.KategOm)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("KATEG_OM");
                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
                entity.Property(e => e.Obec)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OBEC");
                entity.Property(e => e.Okres)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OKRES");
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
