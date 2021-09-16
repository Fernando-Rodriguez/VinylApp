using System.Collections.Generic;
using Newtonsoft.Json;

namespace VinylApp.Domain.DTOs.SpotifyDTOs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Albums
    {
        [JsonProperty("href")]
        public string href { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; } = new List<Item>();

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("previous")]
        public object Previous { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}