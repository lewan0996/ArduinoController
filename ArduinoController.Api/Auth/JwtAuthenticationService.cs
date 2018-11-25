using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ArduinoController.Core.Contract.DataAccess;
using ArduinoController.Core.Exceptions;
using ArduinoController.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ArduinoController.Api.Auth
{
    public class JwtAuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly IRepository<ApplicationUser> _userRepository;
        public JwtAuthenticationService(IConfiguration config, IRepository<ApplicationUser> userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }
        public string GenerateAndSaveRefreshToken(string email)
        {
            var refreshToken = GenerateRefreshToken();
            SaveRefreshToken(email, refreshToken);

            return refreshToken;
        }

        public (string Token, string RefreshToken) Refresh(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            var email = principal.FindFirstValue(ClaimTypes.Email);

            var savedRefreshTokens = GetRefreshTokens(email);

            if (savedRefreshTokens.All(rt => rt.Token != refreshToken))
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            DeleteRefreshToken(email, refreshToken);
            var newRefreshToken = GenerateRefreshToken();
            SaveRefreshToken(email, newRefreshToken);
            var newToken = GenerateToken(principal.Claims);

            return (newToken, newRefreshToken);
        }
        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var jwtConfig = _config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["key"]));

            var jwt = new JwtSecurityToken(
                jwtConfig["Issuer"],
                jwtConfig["Audience"],
                notBefore: DateTime.UtcNow,
                claims: claims.Where(c => c.Type != "iss" && c.Type != "aud"),
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtConfig["MinutesToExpire"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtConfig = _config.GetSection("Jwt");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtConfig["Issuer"],
                ValidAudience = jwtConfig["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ApplicationUser GetUserByEmail(string email)
        {
            var user = _userRepository
                .GetAll(u => u.RefreshTokens)
                .FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                throw new RecordNotFoundException();
            }

            return user;
        }

        private IEnumerable<RefreshToken> GetRefreshTokens(string email)
        {
            var user = GetUserByEmail(email);
            return user.RefreshTokens;
        }

        private void SaveRefreshToken(string email, string token)
        {
            var user = GetUserByEmail(email);
            user.RefreshTokens.Add(new RefreshToken { Token = token });
        }

        private void DeleteRefreshToken(string email, string token)
        {
            var user = GetUserByEmail(email);
            var tokenToDelete = user.RefreshTokens.FirstOrDefault(rt => rt.Token == token);

            if (tokenToDelete == null)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            user.RefreshTokens.Remove(tokenToDelete);
        }
    }
}
