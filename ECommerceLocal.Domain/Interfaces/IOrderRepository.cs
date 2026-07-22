using ECommerceLocal.Domain.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ECommerceLocal.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();

        Task<Order?> GetByIdAsync(string id);

        Task CreateAsync(Order order);

        Task UpdateAsync(string id, Order order);

        Task DeleteAsync(string id);

        Task<List<BsonDocument>> GetOrdersWithProductsAsync();

        Task<List<BsonDocument>> GetTotalSpentPerCustomerAsync();
    }
}
