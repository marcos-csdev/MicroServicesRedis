using Discount.gRPC.Data;
using Discount.gRPC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly DiscountContext _dbContext;

        public DiscountRepository(DiscountContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Coupon?> GetDiscount(string productName)
        {
            if (productName == null) return null;

            var coupon = await _dbContext.Coupon.FirstOrDefaultAsync(cpn => cpn.ProductName == productName);

            return coupon;
        }

        public async Task<int> CreateDiscount(Coupon coupon)
        {
            if (coupon == null) return 0;

            await _dbContext.Coupon.AddAsync(coupon);
            var changes = _dbContext.SaveChanges();

            return changes;
        }

        public int UpdateDiscount(Coupon coupon)
        {
            if (coupon == null) return 0;

            _dbContext.Coupon.Update(coupon);
            var changes = _dbContext.SaveChanges();

            return changes;
        }

        public async Task<int> DeleteDiscount(int id)
        {
            if (id < 1) return 0;

            var coupon = await _dbContext.Coupon.FirstOrDefaultAsync(cpn =>
            cpn.Id == id);

            if (coupon == null) return 0;

            _dbContext.Coupon.Remove(coupon);

            var changes = _dbContext.SaveChanges();

            return changes;

        }
    }
}
