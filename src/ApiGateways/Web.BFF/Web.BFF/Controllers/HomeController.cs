using Microsoft.AspNetCore.Mvc;

namespace Web.BFF.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
