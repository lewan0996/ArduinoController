using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ArduinoController.Api.Dto;
using ArduinoController.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ArduinoController.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public UsersController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(User.Identity.Name);
        }

        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]AuthDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);

            var signInResult =
                await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!signInResult.Succeeded) return Unauthorized();

            var jwtConfig = _configuration.GetSection("Jwt");

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                claims: new List<Claim> { new Claim(ClaimTypes.Name, dto.Email) },
                expires: DateTime.Now.AddMinutes(int.Parse(jwtConfig["MinutesToExpire"])),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new {Token = token});
        }
    }
}