using VinylApp.Domain.Models.VinylAppModels;
using VinylApp.Domain.Models.VinylAppModels.GroupAggregate;
using VinylApp.Infrastructure.Persistence.DbContexts;

namespace VinylApp.Infrastructure.Persistence.Repository
{
    public class GroupRepo : BaseRepo<Group>, IGroupRepo
    {
        public GroupRepo(VinylAppContext context) : base(context)
        {
        }
    }
}
