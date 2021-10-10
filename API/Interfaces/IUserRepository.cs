using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dto;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
  public interface IUserRepository
  {
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string username);
    Task<PagedList<MemberDto>> GetMembersAsync(UsersParams usersParams);
    Task<MemberDto> GetMemberAsync(string username);
  }
}