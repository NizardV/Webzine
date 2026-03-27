using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Webzine.Entity;
using Webzine.EntitiesContext;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Db
{
    public class DbArtisteRepository : IArtisteRepository
    {
        private readonly WebzineDbContext context;
        private readonly ILogger<DbArtisteRepository> logger;

        public DbArtisteRepository(WebzineDbContext context, ILogger<DbArtisteRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void Add(Artiste artiste)
        {
            if (artiste == null)
            {
                this.logger.LogWarning("Tentative d'ajout d'un artiste null.");
                return;
            }

            this.context.Artistes.Add(artiste);
            this.context.SaveChanges();
            this.logger.LogInformation("Artiste ajouté en base (Id: {ArtisteId}).", artiste.IdArtiste);
        }

        public int Count()
        {
            var count = this.context.Artistes.Count();
            this.logger.LogInformation("Comptage des artistes en base: {Count}.", count);
            return count;
        }

        public void Delete(Artiste artiste)
        {
            if (artiste == null)
            {
                this.logger.LogWarning("Tentative de suppression d'un artiste null.");
                return;
            }

            this.context.Artistes.Remove(artiste);
            this.context.SaveChanges();
            this.logger.LogInformation("Artiste supprimé en base (Id: {ArtisteId}).", artiste.IdArtiste);
        }

        public Artiste? Find(int idArtiste)
        {
            var artiste = this.context.Artistes
                .Include(a => a.Titres)
                .FirstOrDefault(a => a.IdArtiste == idArtiste);
            this.logger.LogInformation("Recherche artiste en base par id {ArtisteId}. Trouvé: {Found}.", idArtiste, artiste != null);
            return artiste;
        }

        public IEnumerable<Artiste> FindAll()
        {
            this.logger.LogInformation("Récupération de tous les artistes en base.");
            return this.context.Artistes
                .Include(a => a.Titres)
                .ToList();
        }

        public Artiste? FindByName(string nom)
        {
            if (string.IsNullOrWhiteSpace(nom))
            {
                this.logger.LogWarning("Recherche d'artiste avec un nom vide.");
                return null;
            }

            var artiste = this.context.Artistes
                .Include(a => a.Titres)
                .FirstOrDefault(a => a.Nom.Equals(nom, StringComparison.OrdinalIgnoreCase));
            this.logger.LogInformation("Recherche artiste en base par nom {Nom}. Trouvé: {Found}.", nom, artiste != null);
            return artiste;
        }

        public void Update(Artiste artiste)
        {
            if (artiste == null)
            {
                this.logger.LogWarning("Tentative de mise à jour d'un artiste null.");
                return;
            }

            var existing = this.context.Artistes.FirstOrDefault(a => a.IdArtiste == artiste.IdArtiste);
            if (existing == null)
            {
                this.logger.LogWarning("Artiste introuvable pour mise à jour (Id: {ArtisteId}).", artiste.IdArtiste);
                return;
            }

            existing.Nom = artiste.Nom;
            existing.Biographie = artiste.Biographie;
            this.context.SaveChanges();
            this.logger.LogInformation("Artiste mis à jour en base (Id: {ArtisteId}).", artiste.IdArtiste);
        }
    }
}
