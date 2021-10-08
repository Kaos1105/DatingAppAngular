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

namespace API.Controllers
{
  [Authorize]
  public class UsersController : BaseApiController
  {
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
      _mapper = mapper;
      _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
      var users = await _userRepository.GetMembersAsync();

      return Ok(users);
    }

    // [HttpGet("{id}")]
    // public async Task<ActionResult<AppUser>> GetUser(int id)
    // {
    //   return await _userRepository.GetUserByIdAsync(id);
    // }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
      return await _userRepository.GetMemberAsync(username);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
      var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var user = await _userRepository.GetUserByUsernameAsync(username);

      _mapper.Map(memberUpdateDto, user);

      _userRepository.Update(user);

      if (await _userRepository.SaveAllAsync())
        return NoContent();
      return BadRequest("Fail to update user");
    }
  }
}