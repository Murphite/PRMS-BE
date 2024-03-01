using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Core.Services;
using PRMS.Domain.Entities;

[ApiController]
[Route("api/v1/medical-centers")]
public class MedicalCenterController : Controller
{
    private readonly IMedicalCenterService _medicalCenterService;
    private readonly SignInManager<User> signInManager;

    public MedicalCenterController(IMedicalCenterService medicalCenterService, SignInManager<User> signInManager)
    {
        _medicalCenterService = medicalCenterService;
        this.signInManager = signInManager;
    }



    [HttpGet]
    public async Task<IActionResult> GetAllMedicalCenters([FromQuery] PaginationFilter paginationFilter)
    {
        // Get the current user
        var user = await signInManager.UserManager.GetUserAsync(User);

        // Check if the user object is null
        if (user == null)
        {
            // Return unauthorized response if the user is not authenticated
            return Unauthorized();
        }

        // Get the user's longitude and latitude
        double? userLongitude = user.Address?.Longitude;
        double? userLatitude = user.Address?.Latitude;

        // Call the service method to retrieve all medical centers with pagination
        var result = await _medicalCenterService.GetAll(user.Id, userLongitude, userLatitude, paginationFilter);

        // Check if the operation was successful
        if (result.IsSuccess)
        {
            // Return the paginated list of medical centers
            return Ok(result.Data);
        }

        // Return error response if the operation failed
        return BadRequest(new { ErrorMessage = result.Errors });
    }
}