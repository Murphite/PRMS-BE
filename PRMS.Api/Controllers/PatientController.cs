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

    public PatientController(IPatientService patientService, UserManager<User> userManager)
    {
        _patientService = patientService;
        _userManager = userManager;
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

		private string GetUserId()
		{
			return _userManager.GetUserId(User)!;
		}
	
}