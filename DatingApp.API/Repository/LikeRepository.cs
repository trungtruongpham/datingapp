using System;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public interface ILikeRepository : IBaseRepository<Like>
    {
        Task<Like> GetLike(Guid userId, Guid recipientId);
    }
    public class LikeRepository : BaseRepository<Like>, ILikeRepository
    {
        public LikeRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<Like> GetLike(Guid userId, Guid recipientId)
        {
            return await context.Likes.FirstOrDefaultAsync(u => u.LikerId.Equals(userId) && u.LikeeId.Equals(recipientId));
        }
    }
}