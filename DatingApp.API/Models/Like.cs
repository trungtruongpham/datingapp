using System;

namespace DatingApp.API.Models
{
    public class Like : BaseModel
    {
        public Guid LikerId { get; set; }
        public Guid LikeeId { get; set; }
        public User Liker { get; set; }
        public User Likee { get; set; }
    }
}