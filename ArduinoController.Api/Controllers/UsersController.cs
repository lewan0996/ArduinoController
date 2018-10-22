using System.Security.Claims;
using System.Threading.Tasks;
using ArduinoController.Api.Auth;
using ArduinoController.Api.Dto;
using ArduinoController.Core.Contract.DataAccess;
using ArduinoController.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ArduinoController.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            IAuthenticationService authenticationService, IUnitOfWork unitOfWork)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]AuthDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManager.CreateAsync(new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email
            }, dto.Password);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]AuthDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return Unauthorized();
            }

            var signInResult =
                await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!signInResult.Succeeded) return Unauthorized();

            string token;
            string refreshToken;
            using (var uow = _unitOfWork.Create())
            {
                refreshToken = await _authenticationService.GenerateAndSaveRefreshTokenAsync(dto.Email);
                token = _authenticationService.GenerateToken(new[]
                {
                    new Claim(ClaimTypes.Email, dto.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Id)
                });

                uow.Commit();
            }

            return Ok(new { Token = token, RefreshToken = refreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody]RefreshTokenDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            (string Token, string RefreshToken) newTokens;
            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    newTokens = await _authenticationService.Refresh(dto.Token, dto.RefreshToken);
                    uow.Commit();
                }
                catch (SecurityTokenException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            
            return Ok(new { newTokens.Token, newTokens.RefreshToken });
        }
    }
}