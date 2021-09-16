using Newtonsoft.Json;

namespace VinylApp.Domain.DTOs.SpotifyDTOs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ExternalUrls
    {
        [JsonProperty("spotify")]
        public string Spotify { get; set; }
    }
}