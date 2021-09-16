using System.Collections.Generic;
using System.Security.Claims;

namespace VinylApp.Infrastructure.Services.AuthServices
{
    public interface IAuthService
    {
        string SecretKey { get; set; }
        bool IsTokenValid(string token);
        string TokenGeneration(IAuthModel model);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}