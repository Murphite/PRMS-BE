using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Core.Services;
using PRMS.Domain.Entities;

namespace PRMS.Api.Controllers
{
    public class MedicalCenterController : Controller
    {
        private readonly IMedicalCenterService _medicalCenterService;
        private readonly SignInManager<User> signInManager;

        public MedicalCenterController(IMedicalCenterService medicalCenterService, SignInManager<User> signInManager)
        {
            _medicalCenterService = medicalCenterService;
            this.signInManager = signInManager;
        }
        

        [HttpGet("/api/v1/medical-centers")]
        public async Task<IActionResult> GetAllMedicalCenters([FromQuery] PaginationFilter paginationFilter)
        {
            var user = await signInManager.UserManager.GetUserAsync(User);

            // Call the service method to retrieve all medical centers with pagination
            var result = await _medicalCenterService.GetAll(user.Id, paginationFilter);

            // Check if the operation was successful
            if (result.IsSuccess)
            {
                // Return the paginated list of medical centers
                return Ok(result.Data);
            }
            else
            {
                // Return error response if the operation failed
                return BadRequest(new { ErrorMessage = result.Errors });
            }
        }

    }
}
