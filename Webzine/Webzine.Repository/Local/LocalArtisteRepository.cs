using System;
using System.Collections.Generic;
using System.Linq;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Local
{
    public class LocalArtisteRepository : IArtisteRepository
    {
        private readonly WebzineData _data;
        private readonly ILogger<LocalArtisteRepository> _logger;

        public LocalArtisteRepository(WebzineData data)
        {
            this._data = data;
            this._logger = logger;
        }

        public void Add(Artiste artiste)
        {
            this._logger.LogWarning(
                "Tentative d'ajout d'un artiste dans le repository local. Opération non supportée.");
        }

        public int Count()
        {
            var count = this._data.artistes.Count();
            this._logger.LogDebug("Comptage des artistes: {Count}.", count);
            return count;
        }

        public void Delete(Artiste artiste)
        {
            this._logger.LogWarning(
                "Tentative de suppression d'un artiste dans le repository local. Opération non supportée.");
        }

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
                    $"Erreur lors de la recherche de l'artiste {idArtiste} dans le repository local.");
                throw;
            }
        }

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
                    $"Erreur lors de la recherche de l'artiste par nom {nom} dans le repository local.");
                throw;
            }
        }

        public void Update(Artiste artiste)
        {
            this._logger.LogWarning(
                "Tentative de mise à jour d'un style dans le repository local. Opération non supportée.");
        }
    }
}
