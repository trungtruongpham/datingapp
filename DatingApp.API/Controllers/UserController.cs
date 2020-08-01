using System.Security.Claims;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.DTO;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/user/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IDatingRepository _repo;
        private IMapper _mapper;

        public UserController(IDatingRepository repo, IMapper mapper)
        {
            this._repo = repo;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailDto>(user);

            userToReturn.Age = DateTime.Now.Year - userToReturn.DateOfBirth.Year;

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UserForUpdateDto userForUpdate)
        {
            if (!id.Equals(new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)))    
            {
                return Unauthorized();
            }

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdate, userFromRepo);

            if (await _repo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception("$Updating user {id} failed on save");
        }
    }
}