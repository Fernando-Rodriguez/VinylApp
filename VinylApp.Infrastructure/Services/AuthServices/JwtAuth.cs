using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace VinylApp.Infrastructure.Services.AuthServices
{
    public class JwtAuth : IAuthModel
    {
        private readonly IConfiguration _config;

        public JwtAuth(IConfiguration config)
        {
            _config = config;
        }

        public string SecretKey => _config.GetSection("ServerCredentials").ToString();
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public int ExpireMinutes { get; set; } = 60;
        public Claim[] Claims { get; set; }
    }
}
