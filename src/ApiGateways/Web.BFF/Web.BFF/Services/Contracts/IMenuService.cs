using Menu.API.Grpc;

namespace Web.BFF.Services.Contracts
{
    public interface IMenuService
    {
        Task<IEnumerable<Web.BFF.Models.Pizza>> GetAllPizzasAsync();
    }
}
