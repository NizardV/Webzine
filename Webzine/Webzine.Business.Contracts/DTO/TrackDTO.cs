using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Webzine.Business.Contracts.DTO
{
    public class TrackDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("readable")]
        public bool Readable { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("title_short")]
        public string TitleShort { get; set; }

        [JsonPropertyName("title_version")]
        public string TitleVersion { get; set; }

        [JsonPropertyName("isrc")]
        public string Isrc { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("rank")]
        public int Rank { get; set; }

        [JsonPropertyName("explicit_lyrics")]
        public bool ExplicitLyrics { get; set; }

        [JsonPropertyName("explicit_content_lyrics")]
        public int ExplicitContentLyrics { get; set; }

        [JsonPropertyName("explicit_content_cover")]
        public int ExplicitContentCover { get; set; }

        [JsonPropertyName("preview")]
        public string Preview { get; set; }

        [JsonPropertyName("md5_image")]
        public string Md5Image { get; set; }

        [JsonPropertyName("time_add")]
        public long TimeAdd { get; set; }

        [JsonPropertyName("artist")]
        public ArtistDTO Artist { get; set; }

        [JsonPropertyName("album")]
        public AlbumDTO Album { get; set; }

    }
}
