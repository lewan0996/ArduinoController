using System.Security.Claims;
using System.Threading.Tasks;

namespace ArduinoController.Core.Contract.Auth
{
    public interface IAuthorizationService<T> where T : IOwnedResource
    {
        /// <summary>
        /// Checks if the claimsPrincipal has the permission to modify the resource
        /// </summary>
        /// <param name="claimsPrincipal">ClaimsPrincipal of the user being authorized</param>
        /// <param name="id">Id of an entity</param>
        Task<bool> Authorize(ClaimsPrincipal claimsPrincipal, int id);
    }
}
