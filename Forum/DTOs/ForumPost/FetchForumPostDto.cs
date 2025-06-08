using System.Diagnostics.Contracts;
using Forum.Data.Models.Forum;
using Forum.Mapping;
using Microsoft.AspNetCore.Identity;

namespace Forum.DTOs.ForumPost
{
  public class FetchForumPostDto : IMapFrom<Data.Models.Forum.ForumPost>
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    //public IdentityUser User { get; set; } // cant pass this in too much personal info 
    public string UserName { get; set; }
    public bool IsMisleading { get; set; }
    public int LikesCount { get; set; }
    public List<FetchForumCommentDto> Comments { get; set; }

    public void MapFrom(Data.Models.Forum.ForumPost entity)
    {
      Id = entity.Id;
      Content = entity.Content;
      Title = entity.Title;
      CreatedAt = entity.CreatedAt;
      UpdatedAt = entity.UpdatedAt;
      UserName = entity.User.UserName!;
      LikesCount = entity.LikesCount;
      IsMisleading = entity.IsMisleading;
      Comments = MapFromHelper.MapEntitiesToDtos<ForumComment, FetchForumCommentDto>(entity.Comments.ToList());
    }
  }
}
