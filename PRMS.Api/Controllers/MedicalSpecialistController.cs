using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;

namespace PRMS.Api.Controllers
{
    public class MedicalSpecialistController : Controller
    {
        private readonly IMedicalSpecialistService _medicalSpecialistService;
        private readonly SignInManager<User> signInManager;

        public MedicalSpecialistController(IMedicalSpecialistService medicalSpecialistService, SignInManager<User> signInManager)
        {
            _medicalSpecialistService = medicalSpecialistService;
            this.signInManager = signInManager;
        }


        [HttpGet("/api/v1/medical-specialists")]
        public async Task<IActionResult> GetAllMedicalSpecialists([FromQuery] PaginationFilter paginationFilter)
        {
            var user = await signInManager.UserManager.GetUserAsync(User);

            // Call the service method to retrieve all medical specialists with pagination
            var result = await _medicalSpecialistService.GetAll(user.Id, paginationFilter);

            // Check if the operation was successful
            if (result.IsSuccess)
            {
                // Return the paginated list of medical specialists
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

