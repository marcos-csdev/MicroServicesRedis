using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repositories
{
    public interface IShoppingCartRepository
    {
        Task DeleteCartAsync(string userName);
        Task<ShoppingCart?> GetCartAsync(string userName);
        Task<ShoppingCart?> UpsertCartAsync(ShoppingCart shoppingCart);
    }
}