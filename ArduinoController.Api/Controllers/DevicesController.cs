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
    public class DevicesController : Controller // todo: move logic to service
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

        [HttpPost]
        public IActionResult Add([FromBody] ArduinoDeviceDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var device = dto.MapToArduinoDevice(User.Identity.Name);
            using (var uow = _unitOfWork.Create())
            {
                _devicesRepository.Add(device);
                uow.Commit();
            }

            return Ok(device);
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

        [HttpPut("{id}")] //todo: move to service
        public IActionResult Update(int id, [FromBody]ArduinoDeviceDto dto)
        {
            if (dto == null || id == 0)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var device = _devicesRepository.Get(id);

            if (device == null)
            {
                return NotFound();
            }

            using (var uow = _unitOfWork.Create())
            {
                device.Name = dto.Name;
                device.MacAddress = dto.MacAddress;
                uow.Commit();
            }

            return Ok(device);
        }

        [HttpGet]
        public IActionResult GetAllUserDevices()
        {
            var devices = _devicesRepository.GetAll()
                .Where(d => d.UserId == User.Identity.Name)
                .Select(d => new ArduinoDeviceDto { MacAddress = d.MacAddress, Name = d.Name });

            return Ok(devices);
        }
    }
}