using CatalogAPI.Data;
using CatalogAPI.Models;
using MongoDB.Driver;
using System.Collections;

namespace CatalogAPI.Repositories
{
    public class ProductRepository
    {
        private readonly ICatalogContext _dbContext;

        public ProductRepository(ICatalogContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext)); ;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _dbContext
                .Products
                .Find(prod => true)
                .ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _dbContext
                .Products
                .Find(prod => prod.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByName(string name)
        {
            return await _dbContext
                .Products
                .Find(prod => prod.Name == name)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {

            return await _dbContext
                            .Products
                            .Find(p => p.Category == categoryName)
                            .ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _dbContext
                .Products
                .InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var delete = await _dbContext
                .Products
                .DeleteOneAsync(prod => prod.Id == id);

            return delete.IsAcknowledged && delete.DeletedCount > 0;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var update = await _dbContext
                .Products
                .ReplaceOneAsync(prod => prod.Id == product.Id, replacement: product);

            return update.IsAcknowledged && update.ModifiedCount > 0;
        }

    }
}
