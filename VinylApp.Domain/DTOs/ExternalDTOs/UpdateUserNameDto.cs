using Newtonsoft.Json;

namespace VinylApp.Domain.DTOs.ExternalDTOs
{
    [JsonObject]
    public class UpdateUserNameDto
    {
        [JsonProperty("newUserName")]
        public string NewUserName { get; set; }
    }
}