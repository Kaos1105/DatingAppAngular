using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dto;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using API.Extensions;
using Microsoft.AspNetCore.Http;
using API.Helpers;

namespace API.Controllers
{
  [Authorize]
  public class UsersController : BaseApiController
  {
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
    {
      _mapper = mapper;
      _photoService = photoService;
      _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UsersParams usersParams)
    {
      var users = await _userRepository.GetMembersAsync(usersParams);

      Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalPages, users.TotalCount);

      return Ok(users);
    }

    // [HttpGet("{id}")]
    // public async Task<ActionResult<AppUser>> GetUser(int id)
    // {
    //   return await _userRepository.GetUserByIdAsync(id);
    // }

    [HttpGet("{username}", Name = "GetUser")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
      return await _userRepository.GetMemberAsync(username);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {

      var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

      _mapper.Map(memberUpdateDto, user);

      _userRepository.Update(user);

      if (await _userRepository.SaveAllAsync())
        return NoContent();
      return BadRequest("Fail to update user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
      var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

      var result = await _photoService.AddPhotoAsync(file);

      if (result.Error != null) return BadRequest(result.Error.Message);

      var photo = new Photo
      {
        Url = result.SecureUrl.AbsoluteUri,
        PublicId = result.PublicId,
      };

      if (user.Photos.Count == 0)
      {
        photo.IsMain = true;
      }

      user.Photos.Add(photo);

      if (await _userRepository.SaveAllAsync())
      {
        return CreatedAtRoute("GetUser", new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
      }
      return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
      var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

      var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

      if (photo.IsMain) return BadRequest("This is already your main photo");

      var currentMainPhoto = user.Photos.FirstOrDefault(x => x.IsMain);

      if (currentMainPhoto != null) currentMainPhoto.IsMain = false;

      photo.IsMain = true;

      if (await _userRepository.SaveAllAsync()) return NoContent();

      return BadRequest("Fail to set main photo");
    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
      var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

      var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

      if (photo == null) return NotFound();

      if (photo.IsMain) return BadRequest("You can not delete your main photo");

      if (photo.PublicId != null)
      {
        var result = await _photoService.DeletePhotoAsync(photo.PublicId);
        if (result.Error != null)
          return BadRequest(result.Error.Message);
      }

      user.Photos.Remove(photo);

      if (await _userRepository.SaveAllAsync())
        return Ok();

      return BadRequest("Fail to delete photo");
    }
  }
}