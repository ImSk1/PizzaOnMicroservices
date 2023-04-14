using Menu.API.Infrastructure.Entities;

namespace Menu.API.Services.Contracts
{
    public interface IMenuService
    {
        Task<IEnumerable<Pizza>> GetAllPizzasAsync();
        Task AddPizza(Pizza pizza);
    }
}
