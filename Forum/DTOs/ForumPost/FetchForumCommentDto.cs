using Forum.Data.Models.Forum;
using Forum.Mapping;

namespace Forum.DTOs.ForumPost
{
  public class FetchForumCommentDto : IMapFrom<ForumComment>
  {
    public int Id { get; set; }
    public string Content { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }


    public void MapFrom(ForumComment entity)
    {
      Id = entity.Id;
      Content = entity.CommentContent;
      Username = entity.UserId; 
      CreatedAt = entity.CreatedAt;
    }
  }
}
