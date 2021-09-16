using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace VinylApp.Infrastructure.Services.AuthServices
{
    public class JwtService : IAuthService
    {
        public string SecretKey { get; set; }

        public JwtService(IConfiguration config)
        {
            SecretKey = config.GetSection("ServerCredentials").ToString();
        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token is null or empty");
            }

            var tokenValidationParameters = GetTokenValidationParameters();

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var tokenValid = jwtSecurityTokenHandler.ValidateToken(token,
                tokenValidationParameters,
                out _);

            return tokenValid.Claims;
        }

        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("given token is null or empty");
            }

            var tokenValidationParameters = GetTokenValidationParameters();

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                jwtTokenHandler
                    .ValidateToken(token, tokenValidationParameters, out _);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string TokenGeneration(IAuthModel model)
        {
            if (model?.Claims == null || model.Claims.Length == 0)
            {
                throw new ArgumentException("arguments to create token are not valid.");
            }

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(model.Claims),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(model.ExpireMinutes)),
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), model.SecurityAlgorithm)
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var securityJwtToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            var token = jwtSecurityTokenHandler.WriteToken(securityJwtToken);

            return token;
        }

        //Private methods that create the key and set validation parameters;
        private SecurityKey GetSymmetricSecurityKey()
        {
            var symmetricKey = Encoding.UTF8.GetBytes(SecretKey);

            return new SymmetricSecurityKey(symmetricKey);
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }
    }
}
