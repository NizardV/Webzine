using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Webzine.Business.Contracts.DTO
{
    public class TrackListDTO
    {
        [JsonPropertyName("data")]
        public List<TrackDTO> Tracks { get; set; }
    }
}
