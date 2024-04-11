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
[Authorize(Roles = RolesConstant.Admin)]
[Route("api/v1/admin/prescription")]
public class AdminPrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;
    private readonly IPhysicianService _physicianService;
    private readonly UserManager<User> _userManager;

    public AdminPrescriptionController(IPrescriptionService prescriptionService, IPhysicianService physicianService,
        UserManager<User> userManager)
    {
        _prescriptionService = prescriptionService;
        _physicianService = physicianService;
        _userManager = userManager;
    }

    [HttpPost("{patientUserId}")]
    public async Task<IActionResult> Create([FromRoute] string patientUserId,
        [FromBody] CreatePrescriptionDto prescriptionDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseDto<object>.Failure(ModelState.GetErrors()));

        var physicianUserId = _userManager.GetUserId(User);
        var result = await _prescriptionService.CreatePrescription(patientUserId, physicianUserId!, prescriptionDto);
        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success("Prescription created successfully!"));
    }

    [HttpGet]
    public async Task<IActionResult> GetPhysicianPrescriptions([FromQuery] PaginationFilter? paginationFilter = null)
    {
        paginationFilter ??= new PaginationFilter();
        var userId = _userManager.GetUserId(User);
        var result = await _physicianService.FetchPrescriptions(userId!, paginationFilter);

        if (result.IsFailure)
            return BadRequest(ResponseDto<object>.Failure(result.Errors));

        return Ok(ResponseDto<object>.Success(result.Data));
    }
}