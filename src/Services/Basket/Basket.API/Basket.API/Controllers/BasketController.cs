using Basket.API.Infrastructure.Contracts;
using Basket.API.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Basket.API.Models;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IIdentityService _identityService;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketRepository repository, IIdentityService identityService, ILogger<BasketController> logger)
        {
            _identityService = identityService;
            _repository = repository;
            _logger = logger;
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BuyerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BuyerBasket>> GetBasketByIdAsync(string id)
        {
            var basket = await _repository.GetBasketAsync(id);

            return Ok(basket ?? new BuyerBasket(id));
        }
        [HttpPost]
        [ProducesResponseType(typeof(BuyerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BuyerBasket>> UpdateBasketAsync([FromBody] BuyerBasket value)
        {
            return Ok(await _repository.UpdateBasketAsync(value));
        }
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteBasketByIdAsync(string id)
        {

            var deleteResult = await _repository.DeleteBasketAsync(id);
            if (deleteResult)
            {
                return Ok();
            }
            else
            {
                return BadRequest($"invalid basket id {id}");
            }
        }
    }
}
