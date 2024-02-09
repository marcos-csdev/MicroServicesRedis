
using Discount.gRPC.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Data
{
    public class DiscountContext : DbContext
    {
        public DbSet<Coupon> Coupon { get; } = null!;
        public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //without this command migrations will throw a primary key error 
            base.OnModelCreating(modelBuilder);

        }
    }
}
