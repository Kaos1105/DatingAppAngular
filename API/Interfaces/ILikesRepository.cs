using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dto;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
  public interface ILikesRepository
  {
    Task<UserLike> GetUserLike(int sourceUserId, int LikedUserId);
    Task<AppUser> GetUserWithLikes(int userId);
    Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
  }
}