namespace Basket.API.Grpc.Extensions
{
    public static class BasketGrpcExtensions
    {
        public static Models.BuyerBasket ToDomainModel(this Grpc.BuyerBasket grpcModel)
        {
            return new Models.BuyerBasket
            {
                BuyerId = grpcModel.BuyerId,
                Items = grpcModel.Items.Select(i => i.ToDomainModel()).ToList()
            };
        }

        public static Models.BasketItem ToDomainModel(this Grpc.BasketItem grpcModel)
        {
            return new Models.BasketItem()
            {
                Id = grpcModel.Id,
                PizzaId = grpcModel.PizzaId,
                PizzaName = grpcModel.PizzaName,
                Price = decimal.Parse(grpcModel.Price),
                Quantity = grpcModel.Quantity
            };
        }

        public static Grpc.BuyerBasket ToGrpcModel(this Models.BuyerBasket domainModel)
        {
            var grpcModel = new Grpc.BuyerBasket { BuyerId = domainModel.BuyerId };
            grpcModel.Items.AddRange(domainModel.Items.Select(i => i.ToGrpcModel()));
            return grpcModel;
        }

        public static Grpc.BasketItem ToGrpcModel(this Models.BasketItem domainModel)
        {
            return new Grpc.BasketItem
            {
                Id = domainModel.Id,
                PizzaId = domainModel.PizzaId,
                PizzaName = domainModel.PizzaName,
                Price = domainModel.Price.ToString(),
                Quantity = domainModel.Quantity
            };
        }
    }
}
