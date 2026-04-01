using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;

namespace Webzine.Business.Contracts.DTO
{
    public class AlbumDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("upc")]
        public string Upc { get; set; }

        [JsonPropertyName("cover")]
        public string CoverUrl { get; set; }

        [JsonPropertyName("cover_small")]
        public string CoverSmallUrl { get; set; }

        [JsonPropertyName("cover_medium")]
        public string CoverMediumUrl { get; set; }

        [JsonPropertyName("cover_big")]
        public string CoverBigUrl { get; set; }

        [JsonPropertyName("cover_xl")]
        public string CoverXlUrl { get; set; }

        [JsonPropertyName("md5_image")]
        public string ImageMd5 { get; set; }

        [JsonPropertyName("tracklist")]
        public string TracklistUrl { get; set; }

        [JsonPropertyName("type")]
        public string DataType { get; set; }
    }
}
