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

	[HttpGet("patient-appointment")]
	public async Task<IActionResult> GetPatientAppointments([FromQuery] string? status = null, PaginationFilter? paginationFilter = null)
    {
        paginationFilter ??= new PaginationFilter();
        var physicianUserId = _userManager.GetUserId(User);
        var result = await _adminAppointmentService.GetPatientAppointments(physicianUserId!, status, paginationFilter);

        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success());
    }
    [HttpGet("physician-ranged-appointments")]
	public async Task<IActionResult> GetAllPhysicianRangedAppointments()
    {
        var physicianUserId = _userManager.GetUserId(User);
        var result = await _adminAppointmentService.GetAllPhysicianRangedAppointments(physicianUserId!);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success());
    }

    [HttpGet("monthly-appointments")]
    public async Task<IActionResult> GetMonthlyAppointmentsForYear(string physicianId, PaginationFilter? paginationFilter, string status, int year)
    {
        paginationFilter ??= new PaginationFilter();
        var result = await _adminAppointmentService.GetMonthlyAppointmentsForYear(physicianId, status, year, paginationFilter );
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));
        return Ok(ResponseDto<object>.Success(result.Data));
    }
}