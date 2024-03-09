using Microsoft.AspNetCore.Mvc;

namespace PRMS.Api.Controllers
{
    public class PhysicianController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
