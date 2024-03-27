using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRMS.Api.Dtos;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;

namespace PRMS.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/medical-centers")]
public class MedicalCenterController : Controller
{
    private readonly IMedicalCenterService _medicalCenterService;
    private readonly SignInManager<User> _signInManager;

    public MedicalCenterController(IMedicalCenterService medicalCenterService, SignInManager<User> signInManager)
    {
        _medicalCenterService = medicalCenterService;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMedicalCenters([FromQuery] PaginationFilter paginationFilter)
    {
        // Get the current user
        var user = await _signInManager.UserManager.GetUserAsync(User);

        var userLongitude = user!.Address?.Longitude;
        var userLatitude = user.Address?.Latitude;

        var result = await _medicalCenterService.GetAll(user.Id, userLongitude, userLatitude, paginationFilter);

        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result.Data));
    }
}