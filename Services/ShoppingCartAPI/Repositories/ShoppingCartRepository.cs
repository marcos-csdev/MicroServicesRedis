﻿using StackExchange.Redis;
using Newtonsoft.Json;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IDatabase _redisCache;

        public ShoppingCartRepository(IConnectionMultiplexer redisCache)
        {
            _redisCache = redisCache.GetDatabase();
        }

        public async Task<ShoppingCart?> GetCartAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("user name null when attempting to retrieve cart");

            var redisValue = await _redisCache.StringGetAsync(userName);

            var cart = JsonConvert.DeserializeObject<ShoppingCart>(redisValue.ToString());

            return cart;
        }

        public async Task<ShoppingCart?> UpsertCartAsync(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null) throw new ArgumentNullException("null cart when attempting to update cart");

            var json = JsonConvert.SerializeObject(shoppingCart);

            if (json == null) return null!;

            await _redisCache.StringSetAsync(shoppingCart.UserName, json);

            return await GetCartAsync(shoppingCart.UserName);
        }

        public async Task DeleteCartAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("user name null when attempting to delete cart");

            await _redisCache.KeyDeleteAsync(userName);
        }
    }
}
