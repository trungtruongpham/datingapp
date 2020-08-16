using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DatingApp.API.Helpers;

namespace DatingApp.API.Repository
{
    public interface IDatingRepository
    {
        Task<User> GetUser(Guid id);
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<bool> SaveAll();
        Task<Photo> GetPhoto(Guid id);
        Task<Photo> GetMainPhotoForUser(Guid userId);
        bool DeletePhoto(Photo photoToDelete);
    }
    public class DatingRepository : BaseRepository<User>, IDatingRepository
    {
        public DatingRepository(AppDbContext context) : base(context)
        {
        }

        public bool DeletePhoto(Photo photoToDelete)
        {
            if (photoToDelete == null)
            {
                return false;
            }

            context.Photos.Remove(photoToDelete);

            return true;
        }

        public async Task<Photo> GetMainPhotoForUser(Guid userId)
        {
            return await context.Photos.Where(u => u.UserId.Equals(userId)).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<Photo> GetPhoto(Guid id)
        {
            var photo = await context.Photos.FirstOrDefaultAsync(p => p.Id.Equals(id));

            return photo;
        }

        public async Task<User> GetUser(Guid id)
        {
            var user = await context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id.Equals(id));

            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = context.Users.Include(p => p.Photos);

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}