using System;
using Newtonsoft.Json;

namespace VinylApp.Domain.Models.VinylAppModels.UserAggregate.Auth
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SpotifyToken
    {
        public SpotifyToken()
        {
            DateGenerated = DateTime.UtcNow;
        }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        public DateTime DateGenerated { get; set; }
        public DateTime ExpirationDate => DateGenerated.AddMinutes(ExpiresIn);
    }
}