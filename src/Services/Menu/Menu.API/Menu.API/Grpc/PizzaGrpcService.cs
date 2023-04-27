using Grpc.Core;
using Menu.API.Grpc.Extensions;
using Menu.API.Services.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace Menu.API.Grpc
{
    [Authorize]
    public class PizzaGrpcService : PizzaService.PizzaServiceBase
    {
        private readonly IMenuService _menuService;
        private readonly ILogger<PizzaGrpcService> _logger;

        public PizzaGrpcService(IMenuService menuService, ILogger<PizzaGrpcService> logger)
        {
            _menuService = menuService;
            _logger = logger;
        }

        public override async Task<GetAllPizzasResponse> GetAllPizzas(Empty request, ServerCallContext context)
        {
            var pizzas = await _menuService.GetAllPizzasAsync();
            var response = new GetAllPizzasResponse();
            response.Pizzas.AddRange(pizzas.Select(p => p.ToGrpcPizza()));
            return response;
        }
        public override async Task<Empty> CreatePizza(Pizza request, ServerCallContext context)
        {
            await _menuService.AddPizza(request.ToPizza());
            return new Empty();
        }

        public override async Task<Empty> UpdatePizza(Pizza request, ServerCallContext context)
        {
            await _menuService.UpdatePizza(request.ToPizza());
            return new Empty();
        }

        public override async Task<DeletePizzaResponse> DeletePizza(DeletePizzaRequest request, ServerCallContext context)
        {
            await _menuService.DeletePizza(Guid.Parse(request.Id));
            return new DeletePizzaResponse();
        }
    }
}
