using System.Linq;
using System.Threading.Tasks;
using ArduinoController.Api.Dto;
using ArduinoController.Core.Contract.DataAccess;
using ArduinoController.Core.Models;
using ArduinoController.Core.Models.Commands;
using ArduinoController.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArduinoController.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Procedures")]
    [Authorize]
    public class ProceduresController : Controller
    {
        private readonly IRepository<ArduinoDevice> _devicesRepository;
        private readonly IRepository<Command> _commandsRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ProceduresController(IRepository<Procedure> proceduresRepository, IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager, IRepository<Command> commandsRepository,
            IRepository<ArduinoDevice> devicesRepository)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _commandsRepository = commandsRepository;
            _devicesRepository = devicesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProcedureDto dto) //todo move to separate service
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

            var commands = _commandsRepository.GetAll()
                .Where(c => dto.CommandIds.Contains(c.Id))
                .ToArray();

            var device = dto.DeviceMacAddress == null
                ? null
                : _devicesRepository.GetAll().FirstOrDefault(d => d.MacAddress == dto.DeviceMacAddress);

            var procedure = new Procedure {Name = dto.Name, Commands = commands, Device = device};
            using (var uow = _unitOfWork.Create())
            {
                user.Procedures.Add(procedure);

                uow.Commit();
            }

            return Ok(procedure);
        }
    }
}