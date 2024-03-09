using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace PRMS.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/physicians")]    
public class PhysicianController : ControllerBase
{
}
