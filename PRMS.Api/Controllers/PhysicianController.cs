using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRMS.Api.Dtos;
using PRMS.Core.Abstractions;

namespace PRMS.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/physicians")]    
public class PhysicianController : ControllerBase
{
    private readonly IPhysicianService _physicianService;

    public PhysicianController(IPhysicianService physicianService)
    {
        _physicianService = physicianService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPhysicianReviews(string physicianId)
    {
        var result = await _physicianService.GetPhysicianReviewsAsync(physicianId);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(ResponseDto<object>.Success(result.Data));
    }
}
