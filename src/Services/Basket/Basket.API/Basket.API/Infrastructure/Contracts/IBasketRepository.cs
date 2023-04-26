using Basket.API.Models;

namespace Basket.API.Infrastructure.Contracts
{
    public interface IBasketRepository
    {
        Task<bool> DeleteBasketAsync(string basketId);
        IEnumerable<string> GetBuyerIds();
        Task<BuyerBasket> GetBasketAsync(string customerId);
        Task<BuyerBasket> UpdateBasketAsync(BuyerBasket basket);
    }
}
