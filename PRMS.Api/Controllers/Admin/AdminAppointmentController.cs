using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRMS.Api.Dtos;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Core.Services;
using PRMS.Domain.Constants;
using PRMS.Domain.Entities;

namespace PRMS.Api.Controllers.Admin;

[Route("api/v1/admin/appointment")]
[Authorize(Roles = RolesConstant.Admin)]
[ApiController]
public class AdminAppointmentController : ControllerBase
{
    private readonly IAdminAppointmentService _adminAppointmentService;
    private readonly UserManager<User> _userManager;

    public AdminAppointmentController(IAdminAppointmentService adminAppointmentService, UserManager<User> userManager)
    {
        _adminAppointmentService = adminAppointmentService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetPatientAppointments([FromQuery] string? status = null,
        [FromQuery] PaginationFilter? paginationFilter = null)
    {
        var physicianUserId = _userManager.GetUserId(User);
        paginationFilter ??= new PaginationFilter();
        var result = await _adminAppointmentService.GetPatientAppointments(physicianUserId!, status, paginationFilter);

        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result.Data));
    }

    [HttpGet("physician-ranged-appointments")]
    public async Task<IActionResult> GetAllPhysicianRangedAppointments()
    {
        var physicianUserId = _userManager.GetUserId(User);
        var result = await _adminAppointmentService.GetCurrentMonthAppointment(physicianUserId!);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result.Data));
    }

    [HttpGet("monthly-appointments")]
    public async Task<IActionResult> GetMonthlyAppointmentsForYear(string? status, int year)
    {
        var physicianUserId = _userManager.GetUserId(User);
        var result =
            await _adminAppointmentService.GetMonthlyAppointmentCountForYear(physicianUserId!, status, year);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result.Data));
    }

    [HttpGet("physician-date-appointments")]
    public async Task<IActionResult> GetAllPhysicianAppointmentsSortedByDate(
        [FromQuery] PaginationFilter? paginationFilter)
    {
        var physicianUserId = _userManager.GetUserId(User);
        paginationFilter ??= new PaginationFilter();
        var result =
            await _adminAppointmentService.GetAllPhysicianAppointmentsSortedByDate(physicianUserId!, paginationFilter);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result.Data));
    }
}