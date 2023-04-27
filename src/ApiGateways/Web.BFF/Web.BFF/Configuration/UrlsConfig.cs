namespace Web.BFF.Configuration
{
    public class UrlsConfig
    {
        public class MenuOperations
        {
            public static string GetAllPizzas => $"/api/v1/Pizza/pizzas";

        }

        public class BasketOperations
        {
            
        }



        public string Basket { get; init; } = null!;

        public string Menu { get; init; } = null!;

        public string GrpcBasket { get; init; } = null!;

        public string GrpcMenu { get; init; } = null!;

    }
}
