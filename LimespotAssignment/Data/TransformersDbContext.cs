using LimespotAssignment.Data.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LimespotAssignment.Data
{
    public class TransformersDbContext : DbContext
    {
        public TransformersDbContext(DbContextOptions<TransformersDbContext> options)
            : base(options)
        {
            Database.Migrate();
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Transformer> Transformers { get; set; }

        public DbSet<SpecialTransformerNames> SpecialNames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transformer>(entity =>
            {
                entity.HasKey(k => k.ID)
                    .HasName("PK_ID");

                entity.HasIndex(k => k.Name)
                    .ForSqlServerIsClustered(false)
                    .HasName("IX_Name");

            });

            modelBuilder.Entity<SpecialTransformerNames>().HasData(
                new SpecialTransformerNames[2]
                    {
                        new SpecialTransformerNames(){ID = 1,IsActive = true, TransformerName = "Predaking"},
                        new SpecialTransformerNames(){ID = 2,IsActive = true, TransformerName = "Optimus"}

                    });
        }

    }
}
