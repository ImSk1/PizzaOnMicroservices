using FluentValidation;
using Menu.API.Services.Contracts;
using MongoDbGenericRepository;
using Menu.API.Infrastructure.Entities;
using Menu.API.Services;
using Serilog;


namespace Menu.API.Services
{

    public class MenuService : IMenuService
    {
        private readonly IBaseMongoRepository _repo;
        private readonly ILogger<MenuService> _logger;
        private readonly IValidator<Pizza> _validator;
        public MenuService(IBaseMongoRepository repo, ILogger<MenuService> logger, IValidator<Pizza> validator)
        {
            _repo = repo;
            _logger = logger;
            _validator = validator;
        }

        public async Task<IEnumerable<Pizza>> GetAllPizzasAsync()
        {
            _logger.LogInformation("----- Menu Service - Getting All Pizzas");

            return await _repo.GetAllAsync<Pizza>((pizza) => true);
        }

        public async Task AddPizza(Pizza pizza)
        {
            _logger.LogInformation("----- Menu Service - Creating new pizza. Pizza Name: {pizzaName}", pizza.Name);
            var validationResult = _validator.Validate(pizza);
            if (!validationResult.IsValid)
            {
                _logger.LogError(validationResult.ToString());
                throw new ArgumentException();
            }
            await _repo.AddOneAsync<Pizza>(pizza);
        }
    }
}
