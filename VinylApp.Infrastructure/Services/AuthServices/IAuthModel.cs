using System.Security.Claims;

namespace VinylApp.Infrastructure.Services.AuthServices
{
    public interface IAuthModel
    {
        string SecretKey { get; }
        string SecurityAlgorithm { get; set; }
        int ExpireMinutes { get; set; }
        Claim[] Claims { get; set; }
    }
}
