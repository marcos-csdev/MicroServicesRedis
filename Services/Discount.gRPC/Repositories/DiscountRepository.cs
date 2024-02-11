using Dapper;
using Discount.gRPC.Data;
using Discount.gRPC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Discount.gRPC.Repositories
{
    public class DiscountRepository(IConfiguration configuration) : IDiscountRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("npgsql")!;

        public async Task<Coupon> GetDiscountAsync(string productName)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var query = "SELECT * FROM \"Coupon\" WHERE \"ProductName\" = @ProductName";
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
            (query, new { ProductName = productName });


            if (coupon == null)
                return new Coupon
                { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

            return coupon;



        }

        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var affected =
                await connection.ExecuteAsync
                    ("INSERT INTO \"Coupon\" (\"ProductName\", \"Description\", \"Amount\") VALUES ('@ProductName', '@Description', '@Amount')",
                            new { coupon.ProductName, coupon.Description, coupon.Amount });

            if (affected == 0)
                return false;

            return true;
        }


        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var affected = await connection.ExecuteAsync
                    ("UPDATE \"Coupon\" SET \"ProductName\"=@ProductName, \"Description\" = @Description, \"Amount\" = @Amount WHERE \"Id\" = @Id",
                            new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscountAsync(string productName)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var affected = await connection.ExecuteAsync("DELETE FROM \"Coupon\" WHERE \"ProductName\" = @ProductName",
                new { productName });

            if (affected == 0)
                return false;

            return true;
        }
    }
}
