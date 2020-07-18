using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTO
{
    public class UserRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "You must specify password at least 8 character!")]
        public string Password { get; set; }
    }
}