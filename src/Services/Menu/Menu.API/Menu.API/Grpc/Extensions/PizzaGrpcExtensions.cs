namespace Menu.API.Grpc.Extensions
{
    public static class PizzaGrpcExtensions
    {

        public static Menu.API.Infrastructure.Entities.Pizza ToPizza(this Pizza grpcPizza)
        {
            return new Menu.API.Infrastructure.Entities.Pizza
            {
                Id = Guid.Parse(grpcPizza.Id),
                Name = grpcPizza.Name,
                Ingredients = grpcPizza.Ingredients,
                InStock = grpcPizza.InStock,
                Cost = grpcPizza.Cost
            };
        }

        public static Pizza ToGrpcPizza(this Menu.API.Infrastructure.Entities.Pizza pizza)
        {
            return new Pizza
            {
                Id = pizza.Id.ToString(),
                Name = pizza.Name,
                Ingredients = { pizza.Ingredients },
                InStock = pizza.InStock,
                Cost = pizza.Cost
            };
        }



    }
}
