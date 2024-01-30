using CatalogAPI.Data;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MongoDB.Driver;
using System.Collections.Generic;

namespace CatalogAPI.Repositories
{
    public class Repository<TEntity> where TEntity : class, new()
    {
        private readonly CatalogContext _dbContext;
        private DbSet<TEntity> _entity;

        public Repository(CatalogContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext)); ;

            _entity = dbContext.Set<TEntity>();
        }

        public async Task<IList<TEntity>> GetAllAsync()
        {
            return  await _entity.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(string id)
        {
            return await _entity.FindAsync(new string[] {id});
        }

        public async Task<bool> CreateAsync(TEntity entity)
        {
            var result = await _entity.AddAsync(entity);
            _dbContext.SaveChanges();

            return result.State == EntityState.Modified;
        }

        public bool Delete(TEntity entity)
        {
            var result = _entity.Remove(entity);
            _dbContext.SaveChanges();

            return result.State == EntityState.Modified;
        }

        public bool Update(TEntity entity)
        {
            var result = _entity.Update(entity);
            _dbContext.SaveChanges();


            return result.State == EntityState.Modified;
        }

    }
}
