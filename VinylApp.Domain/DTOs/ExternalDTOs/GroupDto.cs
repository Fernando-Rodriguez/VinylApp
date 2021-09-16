using System.Collections.Generic;
using Newtonsoft.Json;

namespace VinylApp.Domain.DTOs.ExternalDTOs
{
    [JsonObject]
    public class GroupDto
    {
        [JsonProperty("groupName")]
        public string GroupName { get; set; }

        [JsonProperty("groupMemberIds")] 
        public List<string> GroupMemberIds { get; set; } = new List<string>();
    }
}