using System;
using System.Collections.Generic;
using System.Text;

namespace Webzine.Repository.Contracts.Caching
{
    /// <summary>
    /// Interface définissant un service de cache générique pour stocker et récupérer des données.
    /// Utilisé pour mettre en cache les données récupérées depuis les APIs externes (ex: Deezer)
    /// et éviter les appels réseau répétés.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Récupère une valeur du cache.
        /// </summary>
        /// <typeparam name="T">Le type de la valeur en cache (ex: PlaylistDto, Artiste, etc.)</typeparam>
        /// <param name="key">La clé unique pour accéder à la valeur (ex: "playlist_123456")</param>
        /// <param name="value">La valeur récupérée du cache (sortie)</param>
        /// <returns>
        /// True si la clé existe dans le cache et n'a pas expiré, False sinon.
        /// Si la clé a expiré, elle est automatiquement supprimée.
        /// </returns>
        /// <example>
        /// if (cacheService.TryGetValue("playlist_123", out PlaylistDto playlist))
        /// {
        ///     // Utiliser la playlist depuis le cache
        /// }
        /// </example>
        bool TryGetValue<T>(string key, out T value);

        /// <summary>
        /// Ajoute ou met à jour une valeur dans le cache avec une durée d'expiration.
        /// </summary>
        /// <typeparam name="T">Le type de la valeur à mettre en cache</typeparam>
        /// <param name="key">La clé unique pour stocker la valeur</param>
        /// <param name="value">La valeur à mettre en cache</param>
        /// <param name="expiration">Durée avant expiration (ex: TimeSpan.FromHours(24))</param>
        /// <example>
        /// var playlist = await deezerApi.GetPlaylist(123456);
        /// cacheService.Set("playlist_123456", playlist, TimeSpan.FromHours(24));
        /// </example>
        void Set<T>(string key, T value, TimeSpan expiration);

        /// <summary>
        /// Supprime une valeur spécifique du cache.
        /// Utile pour invalider les données obsolètes.
        /// </summary>
        /// <param name="key">La clé de la valeur à supprimer</param>
        void Remove(string key);

        /// <summary>
        /// Vide complètement le cache.
        /// Toutes les données en cache sont supprimées.
        /// Utilisé lors des réinitialisations ou changements de configuration.
        /// </summary>
        void Clear();
    }
}
