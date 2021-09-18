using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VinylApp.Api.Services;
using VinylApp.Domain.DTOs.ExternalDTOs;
using VinylApp.Domain.Services;
using VinylApp.Infrastructure.Services.AuthServices;
using VinylApp.Infrastructure.UnitOfWork;

namespace VinylApp.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/token")]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _auth;
        private readonly IUserServices _userService;
        private readonly IConfiguration _config;

        public AuthController(
            ILogger<AuthController> logger,
            IUnitOfWork unitOWork,
            IAuthService auth,
            IUserServices userService,
            IConfiguration config)
        {
            _config = config;
            _logger = logger;
            _unitOfWork = unitOWork;
            _auth = auth;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] UserLoginDTO userLoginDto)
        {
            if (string.IsNullOrEmpty(userLoginDto.UserName) || string.IsNullOrEmpty(userLoginDto.UserPass))
            {
                return BadRequest();
            }

            try
            {
                _logger.LogInformation("searching for user.");
                var userSearch = await _unitOfWork.Users.Find(usr =>
                    usr.UserAuthorization.UserName == userLoginDto.UserName);

                var user = userSearch.First();

                if (!EncryptionService.Verify(userLoginDto.UserPass, user?.UserAuthorization.UserPass) || user == null)
                    return BadRequest();

                var token = GenerateToken(user.UserAuthorization.UserName, user.Id);
                var refreshToken = GenerateRefreshToken(user.UserAuthorization.UserName, user.Id);
                user.UserAuthorization.RefreshToken = refreshToken;
                await _unitOfWork.SaveChanges();

                HttpContext.Response.Cookies.Append(
                    "_bearer",
                    token,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        IsEssential = true,
                        Expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(10))
                    });

                HttpContext.Response.Cookies.Append(
                    "_refresh",
                    refreshToken,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        IsEssential = true,
                        Expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(1))
                    });

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var user = await _userService.RetrieveRefresh(HttpContext);
                var userFromDb = await _unitOfWork.Users.GetById(user.Id);

                if (user.RefreshToken == null) return BadRequest("Must included refresh cookie");

                if (userFromDb.UserAuthorization.RefreshToken != user.RefreshToken)
                    return BadRequest("Invalid refresh token");

                var token = GenerateToken(userFromDb.UserAuthorization.UserName, userFromDb.Id);
                HttpContext.Response.Cookies.Append(
                    "_bearer",
                    token,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        IsEssential = true,
                        Expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(10))
                    });
                return Ok();
            }
            catch (Exception err)
            {
                _logger.LogError(err.ToString());
                return BadRequest("Error with refresh cookie.");
            }
        }

        private string GenerateToken(string userName, int userId)
        {
            var jwt = new JwtAuth(_config)
            {
                Claims = new[]
                {
                    new Claim("user_name", userName),
                    new Claim("user_id", userId.ToString()),
                    new Claim("user_role", "basic")
                }
            };

            var userSpecificToken = _auth.TokenGeneration(jwt);
            return userSpecificToken;
        }

        private string GenerateRefreshToken(string userName, int userId)
        {
            var random = Guid.NewGuid();
            var jwt = new JwtAuth(_config)
            {
                Claims = new []
                {
                    new Claim("user_name", userName),
                    new Claim("user_id", userId.ToString()),
                    new Claim("user_role", "basic"),
                }
            };
            var userSpecificRefresh = _auth.TokenGeneration(jwt);
            return userSpecificRefresh;
        }
    }
}