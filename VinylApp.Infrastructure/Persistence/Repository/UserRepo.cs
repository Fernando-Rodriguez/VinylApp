using VinylApp.Domain.Models.VinylAppModels;
using VinylApp.Domain.Models.VinylAppModels.UserAggregate;
using VinylApp.Infrastructure.Persistence.DbContexts;

namespace VinylApp.Infrastructure.Persistence.Repository
{
    public class UserRepo : BaseRepo<User>, IUserRepo
    {
        public UserRepo(VinylAppContext context) : base(context)
        {
        }
    }
}
