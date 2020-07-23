using System;

namespace DatingApp.API.DTO
{
    public class UserForListDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public int Age { get; set; }
        public DateTime LastActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
    }
}