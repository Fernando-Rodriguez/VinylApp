using System.Collections.Generic;
using Newtonsoft.Json;

namespace VinylApp.Domain.DTOs.InternalDTOs
{
    [JsonObject]
    public class MultipleItemResponseDto
    {
        [JsonProperty("root")]
            public IEnumerable<SingleItemResponseDto> Root { get; set; }
    }
}