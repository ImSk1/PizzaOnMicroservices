using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Web.BFF.Services.Contracts;

namespace Web.BFF.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private  readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }
        [HttpGet]
        [Route("pizzas")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Pizzas()
        {
            var pizzas = await _menuService.GetAllPizzasAsync();
            return Ok(pizzas);
        }
    }
}
