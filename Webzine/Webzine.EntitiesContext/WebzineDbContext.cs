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

        public DbSet<Artiste> Artistes => Set<Artiste>();
        public DbSet<Titre> Titres => Set<Titre>();
        public DbSet<Style> Styles => Set<Style>();
        public DbSet<Commentaire> Commentaires => Set<Commentaire>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Artiste>().ToTable("Artiste");
            modelBuilder.Entity<Titre>().ToTable("Titre");
            modelBuilder.Entity<Style>().ToTable("Style");
            modelBuilder.Entity<Commentaire>().ToTable("Commentaire");

            modelBuilder.Entity<Artiste>()
                .HasMany(a => a.Titres)
                .WithOne(t => t.Artiste)
                .HasForeignKey(t => t.IdArtiste)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Titre>()
                .HasMany(t => t.Commentaires)
                .WithOne(c => c.Titre)
                .HasForeignKey(c => c.IdTitre)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Titre>()
                .HasMany(t => t.Styles)
                .WithMany(s => s.Titres)
                .UsingEntity<Dictionary<string, object>>(
                    "TitreStyle",
                    j => j
                        .HasOne<Style>()
                        .WithMany()
                        .HasForeignKey("IdStyle")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Titre>()
                        .WithMany()
                        .HasForeignKey("IdTitre")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("IdTitre", "IdStyle");
                        j.ToTable("TitreStyle");
                    });

            modelBuilder.Entity<Titre>()
                .Property(t => t.Duree)
                .HasConversion(
                    t => t.ToTimeSpan(),
                    t => TimeOnly.FromTimeSpan(t)
                );

            modelBuilder.Entity<Artiste>()
                .Property(a => a.Nom)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Artiste>()
                .Property(a => a.Biographie)
                .HasColumnType("TEXT");

            modelBuilder.Entity<Titre>()
                .Property(t => t.Libelle)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Titre>()
                .Property(t => t.Album)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Titre>()
                .Property(t => t.Chronique)
                .IsRequired()
                .HasMaxLength(4000);

            modelBuilder.Entity<Titre>()
                .Property(t => t.UrlJaquette)
                .IsRequired()
                .HasMaxLength(250);

            modelBuilder.Entity<Titre>()
                .Property(t => t.UrlEcoute)
                .IsRequired()
                .HasMaxLength(250);

            modelBuilder.Entity<Style>()
                .Property(s => s.Libelle)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Commentaire>()
                .Property(c => c.Auteur)
                .IsRequired()
                .HasMaxLength(30);

            modelBuilder.Entity<Commentaire>()
                .Property(c => c.Contenu)
                .IsRequired()
                .HasMaxLength(1000);

            modelBuilder.Entity<Artiste>()
                .HasIndex(a => a.Nom);

            modelBuilder.Entity<Titre>()
                .HasIndex(t => t.Libelle);

            modelBuilder.Entity<Style>()
                .HasIndex(s => s.Libelle);

            modelBuilder.Entity<Commentaire>()
                .HasIndex(c => c.DateCreation);
        }
    }
}