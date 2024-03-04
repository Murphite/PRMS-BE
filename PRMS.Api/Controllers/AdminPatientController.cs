using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRMS.Api.Dtos;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;

namespace PRMS.Api.Controllers
{
    [Route("api/v1/admin/patient")]
    [ApiController]
    public class AdminPatientController : ControllerBase
    {
        private readonly IAdminPatientService _adminPatientService;

        public AdminPatientController(IAdminPatientService adminPatientService)
        {
            _adminPatientService = adminPatientService;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFromAdmin([FromBody] UpdatePatientFromAdminDto dto, string patientId)
        {
            var result = await _adminPatientService.UpdateFromAdminAsync(dto, patientId);
            if (result.IsFailure)
                return BadRequest(ResponseDto<object>.Failure(result.Errors));

            return Ok(ResponseDto<object>.Success());
        }
    }
}
