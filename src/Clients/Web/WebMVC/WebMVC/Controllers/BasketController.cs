using Microsoft.AspNetCore.Mvc;
using WebMVC.Services.Contracts;

namespace WebMVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketSvc;

        public BasketController(IBasketService basketSvc)
        {
            _basketSvc = basketSvc;
        }

        [Route("basket")]
        public IActionResult Basket()
        {
            return View();
        }
    }
}
