using System.Linq.Expressions;
using Menu.API.Infrastructure.Entities;

namespace Menu.API.Services.Contracts
{
    public interface IMenuService
    {
        Task<IEnumerable<Pizza>> GetAllPizzasAsync();
        Task<IEnumerable<Pizza>> GetAllPizzasAsync(Expression<Func<Pizza, bool>> predicate);

        Task AddPizza(Pizza pizza);

        Task<Pizza> GetPizzaById(Guid id);

        Task UpdatePizza(Pizza newPizza);

        Task DeletePizza(Guid id);
    }
}
