using System.Threading.Tasks;
using ArduinoController.Api.Dto;
using ArduinoController.Core.Contract.Auth;
using ArduinoController.Core.Contract.DataAccess;
using ArduinoController.Core.Contract.Services;
using ArduinoController.Core.Exceptions;
using ArduinoController.Core.Models;
using ArduinoController.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly IAuthorizationService<Procedure> _authorizationService;

        public ProceduresController(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager, IProcedureService procedureService,
            IAuthorizationService<Procedure> authorizationService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _procedureService = procedureService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.GetUserAsync(User);
            return Ok(_procedureService.GetUserProcedures(user.Id));
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
                try
                {
                    _procedureService.Add(procedure);
                    uow.Commit();
                }
                catch (DbUpdateException)
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                catch (RecordNotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
            }

            return Ok(procedure);
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
                    _procedureService.Delete(id);
                    uow.Commit();
                }
                catch (RecordNotFoundException)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProcedureDto dto)
        {
            if (id == 0 || dto == null)
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

            var newProcedure = dto.MapToProcedure();

            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    _procedureService.Update(id, newProcedure);
                    uow.Commit();
                }
                catch (RecordNotFoundException)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }
    }
}