using Microsoft.AspNetCore.Authorization;
using PRMS.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PRMS.Domain.Entities;
using PRMS.Api.Dtos;

namespace PRMS.Api.Controllers
{
    [ApiController]
    [Authorize]
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

        [HttpGet("physician/{physicianUserId}")]
        public async Task<IActionResult> GetPhysicianAppointments([FromRoute] string physicianUserId, [FromQuery] DateTimeOffset startDate, [FromQuery] DateTimeOffset endDate)
        {
            var result = await _appointmentService.GetAppointmentsForPhysician(physicianUserId, startDate, endDate);
            if (result.IsFailure)
                return BadRequest(ResponseDto<object>.Failure(result.Errors));
            return Ok(ResponseDto<object>.Success());
        }
    }
}