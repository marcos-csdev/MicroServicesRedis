using Discount.gRPC.Models;

namespace Discount.gRPC.Repositories
{
    public interface IDiscountRepository
    {
        Task<int> CreateDiscount(Coupon coupon);
        Task<int> DeleteDiscount(int id);
        Task<Coupon?> GetDiscount(string productName);
        int UpdateDiscount(Coupon coupon);
    }
}