using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRMS.Api.Dtos;
using PRMS.Api.Extensions;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Constants;
using PRMS.Domain.Enums;

namespace PRMS.Api.Controllers.Admin;

[Route("api/v1/admin/patient")]
[Authorize(Roles = RolesConstant.Admin)]
[ApiController]
public class AdminPatientController : ControllerBase
{
    private readonly IAdminPatientService _adminPatientService;
    private readonly IPrescriptionService _prescriptionService;

    public AdminPatientController(IAdminPatientService adminPatientService, IPrescriptionService prescriptionService)
    {
        _adminPatientService = adminPatientService;
        _prescriptionService = prescriptionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPatientList([FromQuery] PaginationFilter? paginationFilter = null)
    {
        paginationFilter ??= new PaginationFilter();
        var result = await _adminPatientService.GetListOfPatients(paginationFilter);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result.Data));
    }

    [HttpGet("{patientUserId}")]
    public async Task<IActionResult> GetPatientDetails([FromRoute] string patientUserId)
    {
        var result = await _adminPatientService.GetPatientDetails(patientUserId);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<PatientDetailsDto>.Success(result.Data));
    }

    [HttpPut("{patientUserId}")]
    public async Task<IActionResult> UpdateFromAdmin([FromBody] UpdatePatientFromAdminDto dto,
        [FromRoute] string patientUserId)
    {
        var result = await _adminPatientService.UpdateFromAdminAsync(dto, patientUserId);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success());
    }

    [HttpPost("{patientUserId}")]
    public async Task<IActionResult> CreatePatientFromAdmin([FromBody] CreatePatientFromAdminDto patientDto,
        string patientUserId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ResponseDto<object>.Failure(ModelState.GetErrors()));
        }

        var result = await _adminPatientService.CreatePatient(patientDto, patientUserId);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success());
    }

    [HttpPut("appointment/status")]
    public async Task<IActionResult> UpdateAdminAppointmentStatus([FromBody] AppointmentStatus status,
        [FromRoute] string patientUserId)
    {
        var result = await _adminPatientService.UpdateAdminAppointmentStatus(patientUserId, status);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success());
    }

    [HttpGet("{patientUserId}/medications")]
    public async Task<IActionResult> GetPatientPrescribedMedicationHistory([FromRoute] string patientUserId,
        [FromQuery] PaginationFilter? paginationFilter = null)
    {
        paginationFilter ??= new PaginationFilter();

        var result = await _prescriptionService.GetPatientPrescribedMedicationHistory(patientUserId, paginationFilter);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result));
    }

    [HttpPut("{medicationId}/update-medication-status")]
    public async Task<IActionResult> UpdateMedicationStatus([FromRoute] string medicationId,
        MedicationStatus medicationStatus)
    {
        var result = await _prescriptionService.UpdateMedicationStatus(medicationId, medicationStatus);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result));
    }
    
    [HttpGet("new-patients-count")]
    public async Task<IActionResult> GetNewPatientsCount()
    {
        var result = await _adminPatientService.GetNewPatientsCount();

        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result));
    }

}