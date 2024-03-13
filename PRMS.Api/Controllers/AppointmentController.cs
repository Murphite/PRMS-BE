using PRMS.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PRMS.Core.Dtos;
using Microsoft.AspNetCore.Identity;
using PRMS.Domain.Entities;
using PRMS.Api.Dtos;

namespace PRMS.Api.Controllers
{
    [ApiController]
    [Route("api/v1/appointment")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly UserManager<User> _userManager;

        public AppointmentController(IAppointmentService appointmentService, UserManager<User> userManager)
        {
            _appointmentService = appointmentService;
            _userManager = userManager;
        }

        [HttpGet("/physician/{physicianId}")]
        public async Task<IActionResult> GetPhysicianAppointments([FromRoute] string physicianId, [FromQuery] DateTimeOffset startDate, [FromQuery] DateTimeOffset endDate)
        {
            var userId = _userManager.GetUserId(User);
            var result = _appointmentService.GetAppointmentsForPhysician(physicianId, startDate, endDate);
            if (result.Result.IsFailure)
                return BadRequest(ResponseDto<object>.Failure(result.Result.Errors));
            return Ok(ResponseDto<object>.Success());
        }
    }
}