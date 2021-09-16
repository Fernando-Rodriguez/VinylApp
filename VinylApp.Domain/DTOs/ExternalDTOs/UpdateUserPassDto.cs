using Newtonsoft.Json;

namespace VinylApp.Domain.DTOs.ExternalDTOs
{
    [JsonObject]
    public class UpdateUserPassDto
    {
        [JsonProperty("newUserPass")]
        public string NewUserPass { get; set; }    
    }
}