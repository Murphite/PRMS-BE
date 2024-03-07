using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRMS.Api.Dtos;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Constants;

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
}