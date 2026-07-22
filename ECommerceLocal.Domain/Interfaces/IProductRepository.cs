using ECommerceLocal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceLocal.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(string id);

        Task<List<Product>> SearchByNameAsync(string name);

        Task<List<Product>> GetByCategoryAsync(string category);

        Task<List<Product>> GetByPriceAsync(decimal minPrice, decimal maxPrice);

        Task CreateAsync(Product product);

        Task UpdateAsync(string id, Product product);

        Task DeleteAsync(string id);
    }
}
