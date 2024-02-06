using Discount.gRPC.Protos;

namespace ShoppingCartAPI.gRPCServices
{
    public class DiscountGrpcService : IDiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _gRPCService;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient gRPCService)
        {
            _gRPCService = gRPCService;
        }
        public async Task<CouponModel> GetDiscountAsync(string productName)
        {
            var request = new GetDiscountRequest { ProductName = productName };

            return await _gRPCService.GetDiscountAsync(request);
        }
    }
}
