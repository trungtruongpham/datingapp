using System;

namespace DatingApp.API.Models
{
    public class Photo : BaseModel
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}