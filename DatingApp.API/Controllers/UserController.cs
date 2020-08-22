using System.Security.Claims;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.DTO;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Controllers
{
    [Route("api/user/[action]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(LogUserActivity))]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;
        private ILikeRepository _likeRepository;

        public UserController(IUserRepository userRepository, IMapper mapper, ILikeRepository likeRepository)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
            this._likeRepository = likeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams)
        {
            var currentUserid = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _userRepository.GetUser(currentUserid);

            userParams.UserId = currentUserid;

            if (!string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }


            var users = await _userRepository.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userRepository.GetUser(id);

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

            var userFromRepo = await _userRepository.GetUser(id);

            _mapper.Map(userForUpdate, userFromRepo);

            if (await _userRepository.SaveAll())
            {
                return NoContent();
            }

            throw new Exception("$Updating user {id} failed on save");
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(Guid id, Guid recipientId)
        {
            if (!id.Equals(new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)))
            {
                return Unauthorized();
            }

            var like = await _likeRepository.GetLike(id, recipientId);

            if (like != null)
            {
                return BadRequest("You already like this user");
            }

            if (await _userRepository.GetUser(recipientId) == null)
            {
                return NotFound();
            }

            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            if (await _likeRepository.InsertAsync(like))
            {
                return Ok("You liked!");
            }

            return BadRequest();
        }
    }
}