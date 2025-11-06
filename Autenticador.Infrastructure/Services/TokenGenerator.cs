

using Autenticador.Application.Common.Interfaces;
using Autenticador.Application.Features.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Autenticador.Infrastructure.Services
{
    public class TokenGenerator(IConfiguration configuration) : ITokenGenerator
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateAccessToken(int userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"] ?? throw new Exception("Secret do JWT não informado")));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(int userId)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = getUniqueToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            return refreshToken;

            static string getUniqueToken()
            {
                var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

                //var tokenIsUnique = !_context.Users.Any(u => u.RefreshTokens.Any(t => t.Token == token));

                //if (!tokenIsUnique)
                //    return getUniqueToken();

                return token;
            }
        }
    }
}
