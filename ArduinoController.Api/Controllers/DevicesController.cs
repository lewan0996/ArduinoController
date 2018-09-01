using System.Linq;
using System.Threading.Tasks;
using ArduinoController.Api.Dto;
using ArduinoController.Core.Contract.DataAccess;
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
        private readonly IRepository<ArduinoDevice> _devicesRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public DevicesController(IRepository<ArduinoDevice> devicesRepository, IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _devicesRepository = devicesRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] ArduinoDeviceDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            using (var uow = _unitOfWork.Create())
            {
                var user = await _userManager.GetUserAsync(User);
                var device = new ArduinoDevice { MacAddress = dto.MacAddress, Name = dto.Name };

                user.Devices.Add(device);

                uow.Commit();
            }

            return NoContent();
        }

        [HttpDelete("{macAddress}")]
        public async Task<IActionResult> Delete(string macAddress)
        {
            if (macAddress == null)
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);
            var device = user.Devices.FirstOrDefault(d => d.MacAddress == macAddress);
            if (device == null)
            {
                return NotFound();
            }

            using (var uow = _unitOfWork.Create())
            {
                _devicesRepository.Delete(device);
                uow.Commit();
            }

            return NoContent();
        }

        [HttpPut("{macAddress}/ChangeName")]
        public async Task<IActionResult> ChangeName(string macAddress, [FromBody]ArduinoDeviceDto dto)
        {
            if (macAddress == null || dto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            var device = user.Devices.FirstOrDefault(d => d.MacAddress == macAddress);

            if (device == null)
            {
                return NotFound();
            }

            using (var uow = _unitOfWork.Create())
            {
                device.Name = dto.Name;
                uow.Commit();
            }

            return Ok(device);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserDevices()
        {
            var user = await _userManager.GetUserAsync(User);
            var devices = user.Devices;

            return Ok(devices);
        }
    }
}