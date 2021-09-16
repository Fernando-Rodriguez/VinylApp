using Newtonsoft.Json;

namespace VinylApp.Domain.DTOs.SpotifyDTOs
{
    public class SpotifyAlbum
    {
        [JsonProperty("albums")]
        public Albums Albums { get; set; }
    }
}
