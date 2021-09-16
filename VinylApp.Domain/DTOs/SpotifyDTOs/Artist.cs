using Newtonsoft.Json;

namespace VinylApp.Domain.DTOs.SpotifyDTOs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Artist
    {
        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; } = new ExternalUrls();

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string URL { get; set; }
    }
}