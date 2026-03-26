using Microsoft.EntityFrameworkCore;
using Webzine.Entity;

namespace Webzine.EntitiesContext
{
    public class WebzineDbContext : DbContext
    {
        public WebzineDbContext(DbContextOptions<WebzineDbContext> options)
            : base(options)
        {
        }

        public DbSet<Artiste> Artistes { get; set; }
        public DbSet<Titre> Titres { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<Commentaire> Commentaires { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Artiste>()
                .HasMany(a => a.Titres)
                .WithOne(t => t.Artiste)
                .HasForeignKey(t => t.IdArtiste)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Titre>()
                .HasMany(t => t.Commentaires)
                .WithOne(c => c.Titre)
                .HasForeignKey(c => c.IdTitre)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Titre>()
                .HasMany(t => t.Styles)
                .WithMany(s => s.Titres)
                .UsingEntity(j => j.ToTable("TitreStyle"));

            modelBuilder.Entity<Commentaire>()
                .Property(c => c.DateCreation)
                .HasConversion(
                    d => d.ToDateTime(TimeOnly.MinValue),
                    d => DateOnly.FromDateTime(d)
                );

            modelBuilder.Entity<Titre>()
                .Property(t => t.Duree)
                .HasConversion(
                    t => t.ToTimeSpan(),
                    t => TimeOnly.FromTimeSpan(t)
                );

            modelBuilder.Entity<Artiste>()
                .HasIndex(a => a.Nom);

            modelBuilder.Entity<Titre>()
                .HasIndex(t => t.Libelle);

            modelBuilder.Entity<Style>()
                .HasIndex(s => s.Libelle);
        }
    }
}