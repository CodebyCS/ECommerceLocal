using ECommerceLocal.Data.Context;
using ECommerceLocal.Domain.Interfaces;
using ECommerceLocal.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceLocal.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(MongoContext context)
        {
            _products = context.Products;
        }

        public async Task<List<Product>> SearchByNameAsync(string name)
        {
            return await _products
                .Find(p => p.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _products.Find(_ => true).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            return await _products.Find(x => x.Id == ObjectId.Parse(id))
                                  .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetByCategoryAsync(string category)
        {
            return await _products.Find(x => x.Category == category)
                                  .ToListAsync();
        }

        public async Task<List<Product>> GetByPriceAsync(decimal minPrice, decimal maxPrice)
        {
            return await _products.Find(x => x.Price >= minPrice &&
                                             x.Price <= maxPrice)
                                  .ToListAsync();
        }

        public async Task CreateAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task UpdateAsync(string id, Product product)
        {
            await _products.ReplaceOneAsync(
                x => x.Id == ObjectId.Parse(id),
                product);
        }

        public async Task DeleteAsync(string id)
        {
            await _products.DeleteOneAsync(
                x => x.Id == ObjectId.Parse(id));
        }
    }
}
