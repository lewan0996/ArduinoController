using System.Linq;
using System.Threading.Tasks;
using ArduinoController.Api.Dto;
using ArduinoController.Core.Contract.Auth;
using ArduinoController.Core.Contract.DataAccess;
using ArduinoController.Core.Contract.Services;
using ArduinoController.Core.Exceptions;
using ArduinoController.Core.Models;
using ArduinoController.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArduinoController.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Devices")]
    [Authorize]
    public class DevicesController : Controller
    {
        private readonly IDeviceService _deviceService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService<ArduinoDevice> _authorizationService;

        public DevicesController(IUnitOfWork unitOfWork, IDeviceService deviceService,
            UserManager<ApplicationUser> userManager, IAuthorizationService<ArduinoDevice> authorizationService)
        {
            _unitOfWork = unitOfWork;
            _deviceService = deviceService;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ArduinoDeviceDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            var device = dto.MapToArduinoDevice(user.Id);
            using (var uow = _unitOfWork.Create())
            {
                _deviceService.Add(device);
                uow.Commit();
            }

            return Ok(device);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var authorizationResult = await _authorizationService.Authorize(User, id);

            if (!authorizationResult)
            {
                return Forbid();
            }

            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    _deviceService.Delete(id);
                }
                catch (RecordNotFoundException)
                {
                    return NotFound();
                }

                uow.Commit();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]ArduinoDeviceDto dto)
        {
            if (dto == null || id == 0)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorizationResult = await _authorizationService.Authorize(User, id);

            if (!authorizationResult)
            {
                return Forbid();
            }
            
            var device = dto.MapToArduinoDevice();

            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    _deviceService.Update(id, device);
                }
                catch (RecordNotFoundException)
                {
                    return NotFound();
                }

                uow.Commit();
            }

            return Ok(device);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserDevices()
        {
            var user = await _userManager.GetUserAsync(User);
            var devices = _deviceService.GetAllUserDevices(user.Id)
                .Select(d => new ArduinoDeviceDto { MacAddress = d.MacAddress, Name = d.Name });

            return Ok(devices);
        }
    }
}