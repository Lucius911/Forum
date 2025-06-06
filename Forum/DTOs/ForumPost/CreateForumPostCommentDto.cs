using System.ComponentModel.DataAnnotations;
using Forum.Data.Models.Forum;

namespace Forum.DTOs.ForumPost
{
  public class CreateForumPostCommentDto : Mapping.IMapTo<ForumComment>
  {
    [Required]
    public string Comment { get; set; }

    [Required]
    public int ForumPostId { get; set; }

    public string? UserId { get; set; }

    public ForumComment Map()
    {
      return new ForumComment
      {
        CommentContent = this.Comment,
        CreatedAt = DateTime.UtcNow,
        ForumPostId = this.ForumPostId,
        //UpdatedAt = DateTime.UtcNow, don't allow edit of comments so not relevant 
        UserId = this.UserId
      };
    }
  }
}
