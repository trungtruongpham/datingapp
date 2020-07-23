using System;
using DatingApp.API.Models;

namespace DatingApp.API.DTO
{
    public class PhotosForDetailDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public bool IsMain { get; set; }

    }
}