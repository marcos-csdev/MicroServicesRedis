using Discount.gRPC.Protos;

namespace ShoppingCartAPI.gRPCServices
{
    public class DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient gRPCService) : IDiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _gRPCService = gRPCService;

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var request = new GetDiscountRequest { ProductName = productName };

            return await _gRPCService.GetDiscountAsync(request);
        }
    }
}
