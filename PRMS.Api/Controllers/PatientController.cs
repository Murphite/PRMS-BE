using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRMS.Api.Dtos;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;

namespace PRMS.Api.Controllers
{
    [ApiController]
    // [Authorize]
    [Route("api/v1/patients")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly UserManager<User> _userManager;

        public PatientController(IPatientService patientService, UserManager<User> userManager)
        {
            _patientService = patientService;
            _userManager = userManager;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFromPatient([FromBody] UpdatePatientFromPatientDto dto)
        {
            // var userId = _userManager.GetUserId(User);
            var userId = "5f4490e7-65e4-45e7-9c1f-dc03b04a3bb2";
            var result = await _patientService.UpdateFromPatientAsync(dto, userId!);
            if (result.IsFailure)
                return BadRequest(ResponseDto<object>.Failure(result.Errors));

            return Ok(ResponseDto<object>.Success());
        }
    }
}