using ECommerceLocal.Domain.Interfaces;
using ECommerceLocal.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceLocal.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(MongoContext context)
        {
            _users = context.Users;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _users.Find(x => x.Id == ObjectId.Parse(id))
                               .FirstOrDefaultAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _users.Find(x => x.Email == email)
                               .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task UpdateAsync(string id, User user)
        {
            await _users.ReplaceOneAsync(
                x => x.Id == ObjectId.Parse(id),
                user);
        }

        public async Task DeleteAsync(string id)
        {
            await _users.DeleteOneAsync(
                x => x.Id == ObjectId.Parse(id));
        }
    }
}

