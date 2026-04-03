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
    /// <inheritdoc/>
    public class DbArtisteRepository : IArtisteRepository
    {
        private readonly WebzineDbContext context;
        private readonly ILogger<DbArtisteRepository> logger;

        /// <inheritdoc/>
        public DbArtisteRepository(WebzineDbContext context, ILogger<DbArtisteRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public void Add(Artiste artiste)
        {
            try
            {
                if (artiste == null)
                {
                    this.logger.LogWarning("Tentative d'ajout d'un artiste null.");
                    return;
                }

                this.context.Artistes.Add(artiste);
                this.context.SaveChanges();
                this.logger.LogDebug("Artiste ajouté en base (Id: {ArtisteId}).", artiste.IdArtiste);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Erreur lors de l'ajout d'un artiste en base.");
                throw;
            }
        }

        /// <inheritdoc/>
        public int Count()
        {
            try
            {
                var count = this.context.Artistes.Count();
                this.logger.LogDebug("Comptage des artistes en base: {Count}.", count);

                return count;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Erreur lors du comptage des artistes en base.");
                throw;
            }
        }

        /// <inheritdoc/>
        public void Delete(Artiste artiste)
        {
            try
            {
                if (artiste == null)
                {
                    this.logger.LogWarning("Tentative de suppression d'un artiste null.");
                    return;
                }

                this.context.Artistes.Remove(artiste);
                this.context.SaveChanges();
                this.logger.LogDebug("Artiste supprimé en base (Id: {ArtisteId}).", artiste.IdArtiste);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Erreur lors de la suppression d'un artiste en base.");
                throw;
            }
        }

        /// <inheritdoc/>
        public Artiste? Find(int idArtiste)
        {
            try
            {
                var artiste = this.context.Artistes
                    .Include(a => a.Titres)
                    .FirstOrDefault(a => a.IdArtiste == idArtiste);
                this.logger.LogDebug("Recherche artiste en base par id {ArtisteId}. Trouvé: {Found}.", idArtiste, artiste != null);

                return artiste;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Erreur lors de la recherche d'un artiste en base par id {ArtisteId}.", idArtiste);
                throw;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Artiste> FindAll()
        {
            try
            {
                this.logger.LogDebug("Récupération de tous les artistes en base.");

                return this.context.Artistes
                    .OrderBy(a => a.Nom)
                    .Include(a => a.Titres)
                    .ToList();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Erreur lors de la récupération de tous les artistes en base.");
                throw;
            }
        }

        /// <inheritdoc/>
        public Artiste? FindByName(string nom)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nom))
                {
                    this.logger.LogWarning("Recherche d'artiste avec un nom vide.");
                    return null;
                }

                var artiste = this.context.Artistes
                    .OrderBy(a => a.Nom)
                    .Include(a => a.Titres)
                    .FirstOrDefault(a => a.Nom.ToLower() == nom.ToLower());
                this.logger.LogDebug("Recherche artiste en base par nom {Nom}. Trouvé: {Found}.", nom, artiste != null);

                return artiste;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Erreur lors de la recherche d'un artiste en base par nom {Nom}.", nom);
                throw;
            }
        }

        /// <inheritdoc/>
        public void Update(Artiste artiste)
        {
            try
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
                this.logger.LogDebug("Artiste mis à jour en base (Id: {ArtisteId}).", artiste.IdArtiste);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Erreur lors de la mise à jour d'un artiste en base (Id: {ArtisteId}).", artiste?.IdArtiste);
                throw;
            }
        }

        /// <inheritdoc/>
        public int CountWithBiography()
        {
            try
            {
                return this.context.Artistes.Count(a => a.Biographie != null && a.Biographie != "");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Erreur lors du comptage des artistes avec biographie en base.");
                throw;
            }
        }

        /// <inheritdoc/>
        public Artiste GetArtistePlusChronique()
        {
            try
            {
                return this.context.Artistes
                       .AsNoTracking()
                       .OrderByDescending(a => a.Titres.Count(t => t.Chronique != null && t.Chronique != ""))
                       .FirstOrDefault();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Erreur lors de la récupération de l'artiste ayant le plus de chroniques.");
                throw;
            }
        }

        /// <inheritdoc/>
        public Artiste GetArtistePlusAlbums()
        {
            try
            {
                return this.context.Artistes
                       .AsNoTracking()
                       .OrderByDescending(a => a.Titres.Select(t => t.Album).Distinct().Count())
                       .FirstOrDefault();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Erreur lors de la récupération de l'artiste ayant le plus d'albums.");
                throw;
            }
        }
    }
}
