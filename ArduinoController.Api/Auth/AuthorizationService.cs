using System.Security.Claims;
using System.Threading.Tasks;
using ArduinoController.Core.Contract.Auth;
using ArduinoController.Core.Contract.DataAccess;
using ArduinoController.DataAccess;
using Microsoft.AspNetCore.Identity;

namespace ArduinoController.Api.Auth
{
    public class AuthorizationService<T> : IAuthorizationService<T> where T : IOwnedResource
    {
        private readonly IRepository<T> _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthorizationService(IRepository<T> repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<bool> Authorize(ClaimsPrincipal claimsPrincipal, int id)
        {
            var entity = _repository.Get(id);
            var user = await _userManager.GetUserAsync(claimsPrincipal);

            return entity.UserId == user.Id;
        }
    }
}
