using ECommerceLocal.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceLocal.Data.Repositories
{
    public class OrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(MongoContext context)
        {
            _orders = context.Orders;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _orders.Find(_ => true).ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(string id)
        {
            return await _orders.Find(x => x.Id == ObjectId.Parse(id))
                                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
        }

        public async Task UpdateAsync(string id, Order order)
        {
            await _orders.ReplaceOneAsync(
                x => x.Id == ObjectId.Parse(id),
                order);
        }

        public async Task DeleteAsync(string id)
        {
            await _orders.DeleteOneAsync(
                x => x.Id == ObjectId.Parse(id));
        }

        public async Task<List<BsonDocument>> GetOrdersWithProductsAsync()
        {
            var pipeline = new[]
            {
                new BsonDocument("$unwind", "$Items"),
                new BsonDocument("$lookup",
                    new BsonDocument
                    {
                        { "from", "products" },
                        { "localField", "Items.ProductId" },
                        { "foreignField", "_id" },
                        { "as", "Product" }
                    })
            };

            return await _orders.Aggregate<BsonDocument>(pipeline).ToListAsync();
        }

        public async Task<List<BsonDocument>> GetTotalSpentPerCustomerAsync()
        {
            var pipeline = new[]
            {
                new BsonDocument("$group",
                    new BsonDocument
                    {
                        { "_id", "$UserId" },
                        { "TotalSpent",
                            new BsonDocument("$sum","$Total") }
                    })
            };

            return await _orders.Aggregate<BsonDocument>(pipeline).ToListAsync();
        }
    }
}
