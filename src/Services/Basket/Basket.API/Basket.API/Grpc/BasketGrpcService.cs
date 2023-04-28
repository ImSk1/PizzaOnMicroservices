using System.Threading.Tasks;
using Basket.API.Extensions;
using Basket.API.Grpc;
using Basket.API.Grpc.Extensions;
using Basket.API.Infrastructure.Contracts;
using Basket.API.Services.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Basket.API.Services
{
    [Authorize]
    public class BasketGrpcService : Grpc.Basket.BasketBase
    {
        private readonly IBasketRepository _repository;
        private readonly IIdentityService _identityService;
        private readonly ILogger<BasketGrpcService> _logger;

        public BasketGrpcService(IBasketRepository repository, IIdentityService identityService, ILogger<BasketGrpcService> logger)
        {
            _identityService = identityService;
            _repository = repository;
            _logger = logger;
        }

        public override async Task<Grpc.BuyerBasket> GetOrCreateBasketById(GetOrCreateBasketByIdRequest request, ServerCallContext context)
        {
            var basket = await _repository.GetOrCreateBasketAsync(request.BuyerId);

            return basket.ToGrpcModel();
        }

        public override async Task<Grpc.BuyerBasket> UpdateBasket(Grpc.BuyerBasket request, ServerCallContext context)
        {
            var domainModel = request.ToDomainModel();
            var updatedBasket = await _repository.UpdateBasketAsync(domainModel);

            return updatedBasket.ToGrpcModel();
        }

        public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteBasketById(DeleteBasketByIdRequest request, ServerCallContext context)
        {
            var deleteResult = await _repository.DeleteBasketAsync(request.BuyerId);

            if (!deleteResult)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Invalid buyer id {request.BuyerId}"));
            }

            return new Google.Protobuf.WellKnownTypes.Empty();
        }
    }
}