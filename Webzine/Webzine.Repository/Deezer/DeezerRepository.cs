using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Webzine.Business.Contracts.DTO;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Deezer
{
    public class DeezerRepository : IDeezerRepository
    {
        private readonly HttpClient _httpClient;
        public DeezerRepository(HttpClient httpClient) {
            _httpClient = httpClient;
        }
        public async Task<PlaylistDto> GetPlaylistAsync(long playlistId)
        {
            var response = await _httpClient.GetAsync($"/playlist/{playlistId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var playlist = JsonSerializer.Deserialize<PlaylistDto>(content);
                return playlist;
            }
            else
            {
                throw new Exception($"Impossible de retrouver la playlist : {playlistId}. erreur: {response.StatusCode}");
            };
        }

    }
}
