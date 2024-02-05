using Discount.gRPC.Models;

namespace Discount.gRPC.Repositories
{
    public interface IDiscountRepository
    {
        Task<bool> CreateDiscountAsync(Coupon coupon);
        Task<bool> DeleteDiscountAsync(string productName);
        Task<Coupon> GetDiscountAsync(string productName);
        Task<bool> UpdateDiscountAsync(Coupon coupon);
    }
}