using System;
using Newtonsoft.Json;

namespace VinylApp.Domain.DTOs.SpotifyDTOs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Image
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }
}
