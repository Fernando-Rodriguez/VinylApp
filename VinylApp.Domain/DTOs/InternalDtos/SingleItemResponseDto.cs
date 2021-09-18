using Newtonsoft.Json;

namespace VinylApp.Domain.DTOs.InternalDTOs
{
    [JsonObject]
    public class SingleItemResponseDto
    {
        [JsonProperty("item")]
        public object Item { get; set; }
    }
}