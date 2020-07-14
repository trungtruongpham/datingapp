using System;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.DTO;
using DatingApp.API.Models;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.API.Controllers
{
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthRepository authRepository;
        private IConfiguration configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            this.authRepository = authRepository;
            this.configuration = configuration;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto model)
        {
            // validate request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.Username = model.Username.ToLower();

            if (await authRepository.UserExists(model.Username))
            {
                return BadRequest("Username already exists");
            }

            var userToCreate = new User
            {
                UserName = model.Username,
            };

            var createdUser = await authRepository.Register(userToCreate, model.Password);

            return StatusCode(201);
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            var userFromRepo = await authRepository.Login(model.UserName, model.Password);

            if (userFromRepo == null)
            {
                return Unauthorized();
            }

            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(configuration.GetSection("AppSettings:SecretKey").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(tokenHandler.WriteToken(token));
        }
    }
}