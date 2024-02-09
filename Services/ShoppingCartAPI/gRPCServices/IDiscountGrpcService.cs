using Discount.gRPC.Protos;

namespace ShoppingCartAPI.gRPCServices
{
    public interface IDiscountGrpcService
    {
        Task<CouponModel> GetDiscount(string productName);
    }
}