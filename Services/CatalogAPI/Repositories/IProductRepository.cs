using CatalogAPI.Models;

namespace CatalogAPI.Repositories
{
    public interface IProductRepository
    {
        Task CreateProductAsync(Product product);
        Task<bool> DeleteProductAsync(string id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName);
        Task<Product> GetProductByIdAsync(string id);
        Task<Product> GetProductByNameAsync(string name);
        Task<bool> UpdateProductAsync(Product product);
    }
}