using Menu.API.Grpc;
using Menu.API.Grpc.Extensions;
using Web.BFF.Services.Contracts;

namespace Web.BFF.Services
{
    public class MenuService : IMenuService
    {
        private readonly PizzaService.PizzaServiceClient _client;
        private readonly ILogger<MenuService> _logger;

        public MenuService(PizzaService.PizzaServiceClient client, ILogger<MenuService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<IEnumerable<Web.BFF.Models.Pizza>> GetAllPizzasAsync()
        {
            var request = new Empty();
            _logger.LogInformation("grpc request {@request}", request);
            try
            {
                var response = await _client.GetAllPizzasAsync(request);
                _logger.LogInformation("grpc response {@response}", response);

                var pizzas = new List<Web.BFF.Models.Pizza>();
                pizzas.AddRange(response.Pizzas.Select(p => p.ToPizza()));
                return pizzas;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }
    }
}
