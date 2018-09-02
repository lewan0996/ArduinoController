using System.Threading.Tasks;
using ArduinoController.Api.Dto;
using ArduinoController.Core.Contract.DataAccess;
using ArduinoController.Core.Contract.Services;
using ArduinoController.Core.Models;
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
        private readonly IProcedureService _procedureService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ProceduresController(IRepository<Procedure> proceduresRepository, IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager, IProcedureService procedureService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _procedureService = procedureService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProcedureDto dto)
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

            var procedure = dto.MapToProcedure(user.Id);

            using (var uow = _unitOfWork.Create())
            {
                _procedureService.Add(procedure);
                uow.Commit();
            }

            return Ok(procedure);
        }
    }
}