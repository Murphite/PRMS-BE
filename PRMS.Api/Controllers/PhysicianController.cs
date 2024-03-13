using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRMS.Api.Dtos;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;

namespace PRMS.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/physician")]
public class PhysicianController : ControllerBase
{
    private readonly IPhysicianService _physicianService;

    public PhysicianController(IPhysicianService physicianService)
    {
        _physicianService = physicianService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPhysicianDetails(string physicianId)
    {
        var result = await _physicianService.GetDetails(physicianId);

        return Ok(ResponseDto<object>.Success(result));
    }

}
