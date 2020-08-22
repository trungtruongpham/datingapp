using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.DTO;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/user/{userId}/photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly IMapper _mapper;
        private readonly IPhotoRepository _photoRepository;
        private readonly IActionContextAccessor _acctionContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly Cloudinary _cloudinary;

        public PhotoController(IPhotoRepository photoRepository,IUserRepository userRepository, IMapper mapper,
         IOptions<CloudinarySettings> cloudinaryConfig, IActionContextAccessor actionContextAccessor)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _photoRepository = photoRepository;
            _acctionContextAccessor = actionContextAccessor;
            _userRepository = userRepository;

            Account account = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhoto(Guid id)
        {
            var photoFromRepo = await _photoRepository.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(Guid userId, [FromForm] PhotoForCreationDto photoForCreationDto)
        {
            if (!userId.Equals(new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)))
            {
                return Unauthorized();
            }
            var userFromRepo = await _userRepository.GetUser(userId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face"),
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Url.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            if (!userFromRepo.Photos.Any(u => u.IsMain))
            {
                photo.IsMain = true;
            }

            userFromRepo.Photos.Add(photo);

            if (await _userRepository.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("", new { userId = userId, id = photo.Id }, photoToReturn);
            }

            return BadRequest("Could not add photo");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(Guid userId, Guid id)
        {
            if (!userId.Equals(new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)))
            {
                return Unauthorized();
            }

            var user = await _userRepository.GetUser(userId);

            if (!user.Photos.Any(p => p.Id.Equals(id)))
            {
                return Unauthorized();
            }

            var photoFromRepo = await _photoRepository.GetPhoto(id);

            if (photoFromRepo.IsMain)
            {
                return BadRequest("This is already the main photo");
            }

            var currentMainPhoto = await _photoRepository.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _photoRepository.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not set photo to main");
        }

        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(Guid userId, Guid photoId)
        {
            if (!userId.Equals(new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)))
            {
                return Unauthorized();
            }

            var user = await _userRepository.GetUser(userId);

            if (!user.Photos.Any(p => p.Id.Equals(photoId)))
            {
                return Unauthorized();
            }

            var photoFromRepo = await _photoRepository.GetPhoto(photoId);

            if (photoFromRepo.IsMain)
            {
                return BadRequest("You can not delete main photo");
            }

            if (photoFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);
                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result.Equals("ok"))
                {
                    _photoRepository.DeletePhoto(photoFromRepo);
                }
            }
            else
            {
                _photoRepository.DeletePhoto(photoFromRepo);
            }

            if (await _photoRepository.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Failed to delete the photo");
        }
    }
}