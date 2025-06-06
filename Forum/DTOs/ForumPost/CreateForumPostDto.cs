using System.ComponentModel.DataAnnotations;

namespace Forum.DTOs.ForumPost
{
  public class CreateForumPostDto : Mapping.IMapTo<Data.Models.Forum.ForumPost>
  {
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }

    public string? UserId { get; set; }

    //will do if time for FE.
    //public byte[]? Image { get; set; }

    public Data.Models.Forum.ForumPost Map()
    {
      return new Data.Models.Forum.ForumPost
      {
        Title = this.Title,
        Content = this.Content,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        UserId = this.UserId
      };
    }
  }
}
