using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VinylApp.Domain.DTOs.ExternalDTOs;
using VinylApp.Domain.DTOs.InternalDTOs;
using VinylApp.Infrastructure.Persistence.Repository;
using VinylApp.Infrastructure.Services.AuthServices;

namespace VinylApp.Api.Services
{
    public class UserServices : IUserServices
    {
        private readonly ILogger<UserServices> _logger;
        private readonly IAuthService _authService;

        public UserServices(
            ILogger<UserServices> logger,
            IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        public async Task<UserDTO> RetrieveUser(HttpContext context)
        {
            _logger.LogInformation("Pulling user info from token");
            var token = context.Request.Cookies.FirstOrDefault(name => name.Key == "_bearer").Value;
            var result = await Task.Run(() => _authService.GetTokenClaims(token).ToArray());

           return new UserDTO
            {
                Id = result[1].Value,
                UserName = result[0].Value
            };
        }
        
        public async Task<UserRefreshDTO> RetrieveRefresh(HttpContext context)
        {
            _logger.LogInformation("Pulling user info from refresh");
            var token = context.Request.Cookies.FirstOrDefault(name => name.Key == "_refresh").Value;
            var result = await Task.Run(() => _authService.GetTokenClaims(token).ToArray());

            return new UserRefreshDTO
            {
                Id = int.Parse(result[1].Value),
                UserName = result[0].Value,
                RefreshToken = token
            };
        }
    }
}