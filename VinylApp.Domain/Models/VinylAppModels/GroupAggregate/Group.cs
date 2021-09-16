using System.Collections.Generic;
using VinylApp.Domain.Models.CoreModels;
using VinylApp.Domain.Models.VinylAppModels.UserAggregate;

namespace VinylApp.Domain.Models.VinylAppModels.GroupAggregate
{
    public class Group : BaseModel
    {
        public Group()
        {
        }

        public Group(string groupName)
        {
            GroupName = groupName;
        }

        public string GroupName { get; private set; }
        public string GroupInfo { get; set; }
        public virtual List<User> GroupMembers { get; set; } = new List<User>();

        public void AddMembers(User user)
        {
            GroupMembers.Add(user);
        }

        public void UpdateGroupName(string name)
        {
            GroupName = name;
        }
    }
}
