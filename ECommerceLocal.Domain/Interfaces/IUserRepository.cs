using ECommerceLocal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceLocal.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();

        Task<User?> GetByIdAsync(string id);

        Task<User?> GetByEmailAsync(string email);

        Task<List<User>> SearchByNameAsync(string name);

        Task CreateAsync(User user);

        Task UpdateAsync(string id, User user);

        Task DeleteAsync(string id);
    }
}
