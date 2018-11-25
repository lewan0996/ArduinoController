using System.Collections.Generic;
using System.Security.Claims;

namespace ArduinoController.Api.Auth
{
    public interface IAuthenticationService
    {
        string GenerateToken(IEnumerable<Claim> claims);
        string GenerateAndSaveRefreshToken(string email);
        (string Token, string RefreshToken) Refresh(string token, string refreshToken);
    }
}
