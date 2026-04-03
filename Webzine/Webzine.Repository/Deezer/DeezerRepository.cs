using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using NLog;
using Webzine.Business.Contracts.DTO;
using Webzine.Repository.Contracts;
using Webzine.Repository.Contracts.Caching;

namespace Webzine.Repository.Deezer
{
    /// <summary>
    /// Repository pour accéder à l'API Deezer et récupérer les données de playlists.
    /// Intègre un système de caching pour éviter les appels réseau répétés.
    /// </summary>
    public class DeezerRepository : IDeezerRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        // Préfixe utilisé pour les clés de cache des playlists
        private const string PlaylistCacheKeyPrefix = "playlist_";

        // Durée par défaut de conservation des données en cache (24 heures)
        // Évite de recharger les playlists trop souvent mais peut être ajusté selon les besoins
        private static readonly TimeSpan DefaultCacheExpiration = TimeSpan.FromHours(24);

        /// <summary>
        /// Initialise le repository avec le client HTTP et le service de cache.
        /// </summary>
        public DeezerRepository(HttpClient httpClient, ICacheService cacheService) {
            _httpClient = httpClient;
            _cacheService = cacheService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Récupère les données d'une playlist depuis l'API Deezer.
        /// Les données sont mises en cache pour éviter les appels répétés.
        /// </summary>
        /// <param name="playlistId">L'ID de la playlist à récupérer</param>
        /// <returns>Les données de la playlist (morceaux, artistes, etc.)</returns>
        public async Task<PlaylistDto> GetPlaylistAsync(long playlistId)
        {
            // Créer une clé de cache unique pour cette playlist
            var cacheKey = $"{PlaylistCacheKeyPrefix}{playlistId}";

            // Vérifier si la playlist est déjà en cache et n'a pas expiré
            // Si oui, la retourner directement sans appel API
            if (_cacheService.TryGetValue<PlaylistDto>(cacheKey, out var cachedPlaylist))
            {
                _logger.Info($"[CACHE HIT] Playlist {playlistId} récupérée depuis le cache");
                return cachedPlaylist;
            }

            // Si les données ne sont pas en cache, les récupérer depuis l'API Deezer
            _logger.Info($"[CACHE MISS] Playlist {playlistId} non trouvée en cache, appel API Deezer...");
            var response = await _httpClient.GetAsync($"/playlist/{playlistId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var playlist = JsonSerializer.Deserialize<PlaylistDto>(content);

                // Mettre les données en cache pour les appels futurs
                _cacheService.Set(cacheKey, playlist, DefaultCacheExpiration);
                _logger.Info($"[CACHE STORED] Playlist {playlistId} mise en cache pour {DefaultCacheExpiration.TotalHours}h");

                return playlist;
            }
            else
            {
                _logger.Error($"Erreur API Deezer pour playlist {playlistId}: {response.StatusCode}");
                throw new Exception($"Impossible de retrouver la playlist : {playlistId}. erreur: {response.StatusCode}");
            };
        }

    }
}
