using AutoBogus;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Data
{
    public static class ShoppingCartGenerator
    {
        public static IList<ShoppingCart> SeedCarts(int amount)
        {
            return AutoFaker.Generate<ShoppingCart>(amount);
        }
    }
}
