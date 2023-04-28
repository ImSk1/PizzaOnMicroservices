using System.Linq.Expressions;
using FluentValidation;
using Menu.API.Services.Contracts;
using MongoDbGenericRepository;
using Menu.API.Infrastructure.Entities;
using Menu.API.Infrastructure.Exceptions;

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
        public async Task<IEnumerable<Pizza>> GetAllPizzasAsync(Expression<Func<Pizza, bool>> prediacte)
        {
            _logger.LogInformation("----- Menu Service - Getting Pizzas");

            return await _repo.GetAllAsync<Pizza>(prediacte);
        }

        public async Task AddPizza(Pizza pizza)
        {
            _logger.LogInformation("----- Menu Service - Creating new pizza. Pizza Name: {pizzaName}", pizza.Name);

            var validationResult = _validator.Validate(pizza);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.ToString());
            }

            await _repo.AddOneAsync<Pizza>(pizza);
        }

        public async Task<Pizza> GetPizzaById(Guid id)
        {
            _logger.LogInformation("----- Menu Service - Getting pizza by Id. Pizza Id: {pizzaId}", id);

            if (!await _repo.AnyAsync<Pizza>(pz => pz.Id == id))
            {
                throw new NotFoundException($"Pizza with id {id} does not exist.");
            }

            return await _repo.GetOneAsync<Pizza>(pz => pz.Id == id);
        }

        public async Task UpdatePizza(Pizza newPizza)
        {
            _logger.LogInformation("----- Menu Service - Updating pizza. Pizza Id: {pizzaId}", newPizza.Id);

            var oldPizza = _repo.GetOne<Pizza>(pz => pz.Id == newPizza.Id);

            if (oldPizza == null)
            {
                throw new NotFoundException($"Pizza with id {newPizza.Id} does not exist.");
            }

            oldPizza = newPizza;

            await _repo.UpdateOneAsync<Pizza>(oldPizza);
        }

        public async Task DeletePizza(Guid id)
        {
            _logger.LogInformation("----- Menu Service - Deleting pizza. Pizza Id: {pizzaId}", id);

            if (!await _repo.AnyAsync<Pizza>(pz => pz.Id == id))
            {
                throw new NotFoundException($"Pizza with id {id} does not exist.");
            }

            var pizza = await _repo.GetOneAsync<Pizza>(pz => pz.Id == id);

            await _repo.DeleteOneAsync<Pizza>(pizza);
        }
    }
}
