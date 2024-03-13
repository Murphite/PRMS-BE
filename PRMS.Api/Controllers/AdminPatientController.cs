using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRMS.Api.Dtos;
using PRMS.Api.Extensions;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Core.Services;
using PRMS.Domain.Constants;
using PRMS.Domain.Entities;
using PRMS.Domain.Enums;

namespace PRMS.Api.Controllers;

[Route("api/v1/admin/patient")]
[Authorize(Roles = RolesConstant.Admin)]
[ApiController]
public class AdminPatientController : ControllerBase
{
    private readonly IAdminPatientService _adminPatientService;

    public AdminPatientController(IAdminPatientService adminPatientService)
    {
        _adminPatientService = adminPatientService;
       
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateFromAdmin([FromBody] UpdatePatientFromAdminDto dto,
        [FromRoute] string userId)
    {
        var result = await _adminPatientService.UpdateFromAdminAsync(dto, userId);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success());
    }

    [HttpPost("{userId}")]
    public async Task<IActionResult> CreatePatientFromAdmin([FromBody] CreatePatientFromAdminDto patientDto,
        string userId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ResponseDto<object>.Failure(ModelState.GetErrors()));
        }

        var result = await _adminPatientService.CreatePatient(patientDto, userId);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success());
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateAdminAppointmentStatus([FromBody] AppointmentStatus status, [FromRoute] string userId)
    {
        var result = await _adminPatientService.UpdateAdminAppointmentStatus(userId, status);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success());
    }
}