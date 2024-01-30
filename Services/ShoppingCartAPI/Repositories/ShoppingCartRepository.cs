using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IDistributedCache _redisCache;

        public ShoppingCartRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task<ShoppingCart?> GetCartAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("user name null when attempting to retrieve cart");

            var json = await _redisCache.GetStringAsync(userName);

            if (string.IsNullOrWhiteSpace(json)) return null;

            var cart = JsonConvert.DeserializeObject<ShoppingCart>(json);

            return cart;
        }

        public async Task<ShoppingCart?> UpdateCartAsync(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null) throw new ArgumentNullException("null cart when attempting to update cart");

            var json = JsonConvert.SerializeObject(shoppingCart);

            if (json == null) return null!;

            await _redisCache.SetStringAsync(shoppingCart.UserName, json);

            return await GetCartAsync(shoppingCart.UserName);
        }

        public async Task DeleteCartAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("user name null when attempting to delete cart");

            await _redisCache.RemoveAsync(userName);
        }
    }
}
