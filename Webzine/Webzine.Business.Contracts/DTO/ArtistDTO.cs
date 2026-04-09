using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Webzine.Business.Contracts.DTO
{
    public class ArtistDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("link")]
        public string ProfileUrl { get; set; }

        [JsonPropertyName("tracklist")]
        public string TracklistUrl { get; set; }

        [JsonPropertyName("type")]
        public string DataType { get; set; }
    }
}
