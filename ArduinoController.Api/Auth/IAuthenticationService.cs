using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ArduinoController.Api.Auth
{
    public interface IAuthenticationService
    {
        string GenerateToken(IEnumerable<Claim> claims);
        Task<string> GenerateAndSaveRefreshTokenAsync(string email);
        Task<(string Token, string RefreshToken)> Refresh(string token, string refreshToken);
    }
}
