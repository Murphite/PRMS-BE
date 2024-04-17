using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRMS.Api.Dtos;
using PRMS.Core.Abstractions;
using PRMS.Domain.Entities;

namespace PRMS.Api.Controllers;

[Authorize]
[Route("api/v1/user")]
public class UserController: ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IRepository _repository;

    public UserController(UserManager<User> userManager, IRepository repository)
    {
        _userManager = userManager;
        _repository = repository;
    }
    
    [HttpGet]
    public async Task<IActionResult> UserDetails()
    {
        var user = await _userManager.GetUserAsync(User);
        var address = _repository.GetAll<Address>()
            .FirstOrDefault(a => a.Id == user.AddressId);

        return Ok(ResponseDto<object>.Success(new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            Address = new
            {
                address.Street,
                address.City,
                address.State,
                address.Country
            }
        }));
    }
}