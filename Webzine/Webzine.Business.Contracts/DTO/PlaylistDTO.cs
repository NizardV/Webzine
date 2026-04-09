using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Webzine.Business.Contracts.DTO
{

public class PlaylistDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("public")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("is_loved_track")]
        public bool IsLovedTrack { get; set; }

        [JsonPropertyName("collaborative")]
        public bool IsCollaborative { get; set; }

        [JsonPropertyName("nb_tracks")]
        public int NbTracks { get; set; }

        [JsonPropertyName("unseen_track_count")]
        public int UnseenTrackCount { get; set; }

        [JsonPropertyName("fans")]
        public int FansCount { get; set; }

        [JsonPropertyName("link")]
        public string LinkUrl { get; set; }

        [JsonPropertyName("share")]
        public string ShareUrl { get; set; }

        [JsonPropertyName("picture")]
        public string PictureUrl { get; set; }

        [JsonPropertyName("picture_small")]
        public string PictureSmallUrl { get; set; }

        [JsonPropertyName("picture_medium")]
        public string PictureMediumUrl { get; set; }

        [JsonPropertyName("picture_big")]
        public string PictureBigUrl { get; set; }

        [JsonPropertyName("picture_xl")]
        public string PictureXlUrl { get; set; }

        [JsonPropertyName("checksum")]
        public string Checksum { get; set; }

        [JsonPropertyName("tracklist")]
        public string TracklistUrl { get; set; }

        [JsonPropertyName("creation_date")]
        public string CreationDate { get; set; }

        [JsonPropertyName("add_date")]
        public string AddDate { get; set; }

        [JsonPropertyName("mod_date")]
        public string ModDate { get; set; }

        [JsonPropertyName("md5_image")]
        public string ImageMd5 { get; set; }

        [JsonPropertyName("picture_type")]
        public string PictureType { get; set; }

        [JsonPropertyName("type")]
        public string DataType { get; set; }

        [JsonPropertyName("tracks")]
        public TrackListDTO TrackList { get; set; }
    }
}
