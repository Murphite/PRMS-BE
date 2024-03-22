using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRMS.Api.Dtos;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Core.Services;
using PRMS.Domain.Entities;

namespace PRMS.Api.Controllers;

[ApiController]
[Authorize]
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

        // Get the user's longitude and latitude
        double? userLongitude = user.Address?.Longitude;
        double? userLatitude = user.Address?.Latitude;

        // Call the service method to retrieve all medical centers with pagination
        var result = await _medicalCenterService.GetAll(user.Id, userLongitude, userLatitude, paginationFilter);

        // Check if the operation was unsuccessful
        if (result.IsFailure)
            // Return error response if the operation failed
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        // Return the paginated list of medical centers
        return Ok(ResponseDto<object>.Success(result.Data));
    }

}