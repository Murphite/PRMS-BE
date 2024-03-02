using Microsoft.AspNetCore.Mvc;
using PRMS.Api.Dtos;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;

namespace PRMS.Api.Controllers
{
    [ApiController]
    [Route("api/v1/patients")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost("update-from-patient")]
        public async Task<IActionResult> UpdateFromPatient([FromBody] UpdatePatientFromPatientDto dto, string userId)
        {
            var result = await _patientService.UpdateFromPatientAsync(dto, userId);
            if (result.IsFailure)
                return BadRequest(ResponseDto<object>.Failure(result.Errors));

            return Ok(ResponseDto<object>.Success());
        }

        [HttpPost("update-from-doctor")]
        public async Task<IActionResult> UpdateFromDoctor([FromBody] UpdatePatientFromDoctorDto dto, string patientId)
        {
            var result = await _patientService.UpdateFromDoctorAsync(dto, patientId);
            if (result.IsFailure)
                return BadRequest(ResponseDto<object>.Failure(result.Errors));

            return Ok(ResponseDto<object>.Success());
        }
    }
}