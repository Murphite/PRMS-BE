using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRMS.Api.Dtos;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;
using PRMS.Domain.Enums;

namespace PRMS.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/patient")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly UserManager<User> _userManager;
    private readonly IPrescriptionService _prescriptionService;

    public PatientController(IPatientService patientService, UserManager<User> userManager,
        IPrescriptionService prescriptionService)

    {
        _patientService = patientService;
        _userManager = userManager;
        _prescriptionService = prescriptionService;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateFromPatient([FromBody] UpdatePatientFromPatientDto dto)
    {
        var userId = _userManager.GetUserId(User);
        var result = await _patientService.UpdateFromPatientAsync(dto, userId!);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success());
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewPatient([FromBody] CreatePatientFromUserDto patientDto)
    {
        var userId = GetUserId();
        var response = await _patientService.CreatePatient(userId, patientDto);
        if (response.IsFailure)
        {
            return BadRequest(ResponseDto<object>.Failure(response.Errors));
        }

        return Ok(ResponseDto<object>.Success());
    }

    [HttpGet("appointment")]
    public async Task<IActionResult> GetPatientAppointments([FromQuery] string? status = null,
        [FromQuery] PaginationFilter? paginationFilter = null)
    {
        paginationFilter ??= new PaginationFilter();
        var userId = _userManager.GetUserId(User);
        var result = await _patientService.GetPatientAppointments(userId, status, paginationFilter);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result.Data));
    }

    private string GetUserId()
    {
        return _userManager.GetUserId(User)!;
    }

    [HttpGet("medication")]
    public async Task<IActionResult> GetPatientPrescribedMedicationHistory(
        [FromQuery] PaginationFilter? paginationFilter = null)
    {
        paginationFilter ??= new PaginationFilter();

        var patientUserId = _userManager.GetUserId(User);

        var result = await _prescriptionService.GetPatientPrescribedMedicationHistory(patientUserId!, paginationFilter);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result.Data));
    }

    [HttpGet("medication/{medicationId}")]
    public async Task<IActionResult> GetPatientPrescribedMedicationHistory([FromRoute] string medicationId)
    {
        var result = await _prescriptionService.GetPrescribedMedicationHistoryById(medicationId);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result.Data));
    }

    [HttpPut("medication/{medicationId}/update-medication-status")]
    public async Task<IActionResult> UpdateMedicationStatus([FromRoute] string medicationId,
        [FromBody] MedicationStatus medicationStatus)
    {
        var result = await _prescriptionService.UpdateMedicationStatus(medicationId, medicationStatus);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success());
    }
}
