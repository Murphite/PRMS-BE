using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;
using System.Threading.Tasks;

namespace PRMS.Api.Controllers;

[ApiController]
[Route("api/v1/physicans")]
public class PhysicianController : Controller
{
    private readonly IPhysicianService _PhysicanService;
    private readonly SignInManager<User> signInManager;

    public PhysicianController(IPhysicianService PhysicanService, SignInManager<User> signInManager)
    {
        _PhysicanService = PhysicanService;
        this.signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMedicalPhysicians([FromQuery] PaginationFilter paginationFilter)
    {
        var user = await signInManager.UserManager.GetUserAsync(User);

        // Call the service method to retrieve all medical Physicans with pagination
        var result = await _PhysicanService.GetAll(user.Id, paginationFilter);

        // Check if the operation was successful
        if (result.IsSuccess)
        {
            // Return the paginated list of medical Physicans
            return Ok(result.Data);
        }

        // Return error response if the operation failed
        return BadRequest(new { ErrorMessage = result.Errors });
    }

}