using Forum.Mapping;
using Microsoft.AspNetCore.Identity;

namespace Forum.DTOs.ForumPost
{
  public class FetchForumPostDto : IMapFrom<Data.Models.Forum.ForumPost>
  {
    public int Id { get; set; }

    public string UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    //public IdentityUser User { get; set; } // cant pass this in too much personal info 
    public string UserName { get; set; }

    public void MapFrom(Data.Models.Forum.ForumPost entity)
    {
      Content = entity.Content;
      Title = entity.Title;
      CreatedAt = entity.CreatedAt;
      UpdatedAt = entity.UpdatedAt;
      UserName = entity.User.UserName!;
    }
  }
}
