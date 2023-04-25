using Microsoft.AspNetCore.Mvc;
using System.Net;
using Menu.API.Infrastructure.Entities;
using Menu.API.Services.Contracts;

namespace Menu.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public PizzaController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [Route("pizzas")]
        [ProducesResponseType(typeof(IEnumerable<Pizza>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PizzasAsync()
        {
            var pizzas = await _menuService.GetAllPizzasAsync();
            return Ok(pizzas);
        }

        [Route("pizzas")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreatePizzaAsync([FromBody] Pizza pizza)
        {
            var newPizza = new Pizza
            {
                Name = pizza.Name,
                InStock = pizza.InStock,
                Ingredients = pizza.Ingredients,
                Cost = pizza.Cost,

            };
            await _menuService.AddPizza(newPizza);
            return CreatedAtAction("PizzasAsync", null);
        }

        [Route("pizzas")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdatePizzaAsync([FromBody] Pizza pizzaToUpdate)
        {
            await _menuService.UpdatePizza(pizzaToUpdate);
            return CreatedAtAction("PizzasAsync", null);
        }

        [Route("pizzas")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> DeletePizzaAsync([FromBody] Guid id)
        {
            await _menuService.DeletePizza(id);

            return CreatedAtAction("PizzasAsync", null);
        }
    }
}
