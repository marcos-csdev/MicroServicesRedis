using Discount.gRPC.Protos;

namespace ShoppingCartAPI.gRPCServices
{
    public interface IDiscountGrpcService
    {
        Task<CouponModel> GetDiscountAsync(string productName);
    }
}