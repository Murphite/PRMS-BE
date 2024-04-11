using Microsoft.AspNetCore.Authorization;
using PRMS.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PRMS.Domain.Entities;
using PRMS.Api.Dtos;
using PRMS.Core.Dtos;
using PRMS.Domain.Enums;
using PRMS.Core.Services;

namespace PRMS.Api.Controllers;

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

    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto appointmentDto)
    {
        var userId = _userManager.GetUserId(User);
        var result = await _appointmentService.CreateAppointment(userId!, appointmentDto);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));
        return Ok(ResponseDto<object>.Success("Appointment created successfully!"));
    }

    [HttpGet("physician/{physicianUserId}")]
    public async Task<IActionResult> GetPhysicianAppointments([FromRoute] string physicianUserId,
        [FromQuery] DateTimeOffset startDate, [FromQuery] DateTimeOffset endDate)
    {
        var result = await _appointmentService.GetAppointmentsForPhysician(physicianUserId, startDate, endDate);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));
        return Ok(ResponseDto<object>.Success(result));
    }

    [HttpPut("{appointmentId}/status")]
    public async Task<IActionResult> UpdateAppointmentStatus([FromRoute] string appointmentId,
        [FromBody] AppointmentUpdateDto data)
    {
        var status = (AppointmentStatus)data.Status;
        var userId = _userManager.GetUserId(User);
        var result = await _appointmentService.UpdateAppointmentStatus(userId!, appointmentId, status);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success());
    }

    [HttpPut("{appointmentId}/reschedule")]
    public async Task<IActionResult> RescheduleAppointment([FromRoute] string appointmentId,
        [FromBody] RescheduleAppointmentDto rescheduleDto)
    {
        var userId = _userManager.GetUserId(User);
        var result = await _appointmentService.RescheduleAppointment(appointmentId, rescheduleDto);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));
        return Ok(ResponseDto<object>.Success());
    }

    [HttpGet("{physicianId}/appointments/total")]
    public async Task<IActionResult> GetTotalAppointmentsForDay(string physicianId, [FromQuery] DateTimeOffset? date)
    {
        // If date is not provided, default it to the current day
        var currentDate = date ?? DateTimeOffset.Now;

        var result = await _appointmentService.GetTotalAppointmentsForDay(physicianId, currentDate);

        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));
       
        return Ok(ResponseDto<object>.Success(result));

    }
}

public class AppointmentUpdateDto
{
    public int Status { get; set; }
}