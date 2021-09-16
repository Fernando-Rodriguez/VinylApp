using VinylApp.Domain.Models.CoreModels;

namespace VinylApp.Domain.Models.VinylAppModels.UserAggregate.Auth
{
    public class AuthUser : BaseModel
    {
        public AuthUser() {}
        public AuthUser(string user, string pass)
        {
            UserName = user;
            UserPass = pass;
        }

        public string UserName { get; set; }
        public string UserPass { get; set; }
        public string RefreshToken { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
