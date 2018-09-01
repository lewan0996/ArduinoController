using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ArduinoController.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ArduinoController.Api.Auth
{
    public class JwtAuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        public JwtAuthenticationService(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }
        public async Task<string> GenerateAndSaveRefreshTokenAsync(string email)
        {
            var refreshToken = GenerateRefreshToken();
            await SaveRefreshTokenAsync(email, refreshToken);

            return refreshToken;
        }

        public async Task<(string Token, string RefreshToken)> Refresh(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            var email = principal.Identity.Name;

            var savedRefreshToken = await GetRefreshTokenAsync(email);

            if (savedRefreshToken != refreshToken)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            await DeleteRefreshTokenAsync(email);
            var newRefreshToken = GenerateRefreshToken();
            await SaveRefreshTokenAsync(email, newRefreshToken);
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
                notBefore: DateTime.Now,
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(jwtConfig["MinutesToExpire"])),
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

        private async Task<string> GetRefreshTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user.RefreshToken;
        }

        private async Task SaveRefreshTokenAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            user.RefreshToken = token;
        }

        private async Task DeleteRefreshTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            user.RefreshToken = null;
        }
    }
}
