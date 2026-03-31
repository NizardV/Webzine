using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Local
{
    /// <inheritdoc/>
    public class LocalArtisteRepository : IArtisteRepository
    {
        private readonly WebzineData _data;
        private readonly ILogger<LocalArtisteRepository> _logger;

        /// <inheritdoc/>
        public LocalArtisteRepository(WebzineData data, ILogger<LocalArtisteRepository> logger)
        {
            this._data = data;
            this._logger = logger;
        }

        /// <inheritdoc/>
        public void Add(Artiste artiste)
        {
            this._logger.LogWarning(
                "Tentative d'ajout d'un artiste dans le repository local. Opération non supportée.");
        }

        /// <inheritdoc/>
        public int Count()
        {
            var count = this._data.artistes.Count();
            this._logger.LogDebug("Comptage des artistes: {Count}.", count);
            return count;
        }

        /// <inheritdoc/>
        public void Delete(Artiste artiste)
        {
            this._logger.LogWarning(
                "Tentative de suppression d'un artiste dans le repository local. Opération non supportée.");
        }

        /// <inheritdoc/>
        public Artiste? Find(int idArtiste)
        {
            try
            {
                this._logger.LogDebug(
                    "Recherche du style d'identifiant {IdStyle} dans le repository local.",
                    idArtiste);

                var artiste = this._data.artistes.FirstOrDefault(a => a.IdArtiste == idArtiste);
                this._logger.LogDebug("Recherche artiste par id {ArtisteId}. Trouvé: {Found}.", idArtiste, artiste != null);
                return artiste;
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la recherche de l'artiste {idArtiste} dans le repository local.", idArtiste);
                throw;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Artiste> FindAll()
        {
            try
            {
                this._logger.LogDebug(
                    "Récupération de tous les artistes depuis le repository local.");

                return this._data.artistes.ToList();
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la récupération de tous les artistes depuis le repository local.");
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
                    this._logger.LogWarning("Recherche d'artiste avec un nom vide.");
                    return null;
                }

                var artiste = this._data.artistes.FirstOrDefault(a => string.Equals(a.Nom, nom, StringComparison.OrdinalIgnoreCase));
                this._logger.LogDebug("Recherche artiste par nom {Nom}. Trouvé: {Found}.", nom, artiste != null);

                return artiste;
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la recherche de l'artiste par nom {nom} dans le repository local.", nom);
                throw;
            }
        }

        /// <inheritdoc/>
        public void Update(Artiste artiste)
        {
            this._logger.LogWarning(
                "Tentative de mise à jour d'un style dans le repository local. Opération non supportée.");
        }

        /// <inheritdoc/>
        public int CountWithBiography()
        {
            try
            {
                return this._data.artistes.Count(a => a.Biographie != null && a.Biographie != "");
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du comptage des artistes avec biographie en base.");
                throw;
            }
        }

        /// <inheritdoc/>
        public Artiste GetArtistePlusChronique()
        {
            try
            {
                return this._data.artistes
                       .OrderByDescending(a => a.Titres.Count(t => t.Chronique != null && t.Chronique != ""))
                       .FirstOrDefault();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération de l'artiste ayant le plus de chroniques.");
                throw;
            }
        }


        /// <inheritdoc/>
        public Artiste GetArtistePlusAlbums()
        {
            try
            {
                return this._data.artistes
                       .OrderByDescending(a => a.Titres.Select(t => t.Album).Distinct().Count())
                       .FirstOrDefault();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération de l'artiste ayant le plus d'albums.");
                throw;
            }
        }

    }
}
