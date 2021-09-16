using System.Collections.Generic;
using Newtonsoft.Json;

namespace VinylApp.Domain.DTOs.SpotifyDTOs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Item
    {
        [JsonProperty("album_type")]
        public string AlbumTypes { get; set; }

        [JsonProperty("artists")]
        public List<Artist> Artists { get; set; } = new List<Artist>();

        [JsonProperty("external_urls")]
        public ExternalUrls External_urls { get; set; } = new ExternalUrls();

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; } = new List<Image>();

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("release_date_precision")]
        public string ReleaseDatePrecision { get; set; }

        [JsonProperty("total_tracks")]
        public int TotalTracks { get; set; }

        [JsonProperty("type")]
        public string Types { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
