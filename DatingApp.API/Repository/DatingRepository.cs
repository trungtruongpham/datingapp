using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public interface IDatingRepository
    {
        Task<User> GetUser(Guid id);
        Task<IEnumerable<User>> GetUsers();
        Task<bool> SaveAll();
    }
    public class DatingRepository : BaseRepository<User>, IDatingRepository
    {
        public DatingRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User> GetUser(Guid id)
        {
            var user = await context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id.Equals(id));
            
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await context.Users.Include(p => p.Photos).ToListAsync();
        
            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}