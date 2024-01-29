using CatalogAPI.Models;
using MongoDB.Driver;

namespace CatalogAPI.Data
{
    public class CatalogContext
    {
        public IMongoCollection<Product> Products { get;  }


        public CatalogContext(IConfiguration configuration) 
        {
            var client = new MongoClient(GetConfigValue(configuration, "ConnectionString"));

            var db = client.GetDatabase(GetConfigValue(configuration, "DatabaseName"));

            Products = db.GetCollection<Product>(GetConfigValue(configuration, "CollectionName"));

            CatalogContextSeed.SeedData(Products);

        }

        private string? GetConfigValue(IConfiguration configuration, string value)
        {
            return configuration.GetValue<string>($"DatabaseSettings:{value}");
        }
    }
}
