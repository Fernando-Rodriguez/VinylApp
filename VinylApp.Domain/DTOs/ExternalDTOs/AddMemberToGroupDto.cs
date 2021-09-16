using System.Collections.Generic;

namespace VinylApp.Domain.DTOs.ExternalDTOs
{
    public class AddMemberToGroupDto
    {
        public int GroupId { get; set; }
        public List<string> NewMembers { get; set; }
    }
}