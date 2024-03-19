using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRMS.Api.Dtos;
using PRMS.Api.Extensions;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Constants;
using PRMS.Domain.Entities;

namespace PRMS.Api.Controllers.Admin;

[ApiController]
// [Authorize(Roles = RolesConstant.Admin)]
[Route("api/v1/admin/prescription/{patientUserId}")]
public class AdminPrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;
    private readonly UserManager<User> _userManager;

    public AdminPrescriptionController(IPrescriptionService prescriptionService, UserManager<User> userManager)
    {
        _prescriptionService = prescriptionService;
        _userManager = userManager;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] string patientUserId, [FromBody] CreatePrescriptionDto prescriptionDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseDto<object>.Failure(ModelState.GetErrors()));
        // var physicianUserId = _userManager.GetUserId(User);
        var physicianUserId = "59fd28b1-4f82-4f9e-bdc0-80661d850e42";
        var result = await _prescriptionService.CreatePrescription(patientUserId, physicianUserId!, prescriptionDto);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));
        
        return Ok(ResponseDto<object>.Success("Prescription created successfully!"));
    }
}