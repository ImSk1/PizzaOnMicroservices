using Menu.API.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Menu.API.Controllers
{
    
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
