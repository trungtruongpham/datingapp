using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public interface IPhotoRepository : IBaseRepository<Photo>
    {
        Task<Photo> GetPhoto(Guid id);
        Task<Photo> GetMainPhotoForUser(Guid userId);
        bool DeletePhoto(Photo photoToDelete);
    }
    public class PhotoRepository : BaseRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(AppDbContext context) : base(context)
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
    }
}