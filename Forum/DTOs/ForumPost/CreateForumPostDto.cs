using System.ComponentModel.DataAnnotations;

namespace Forum.DTOs.ForumPost
{
  public class CreateForumPostDto : Mapping.IMapTo<Data.Models.ForumPost>
  {
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }

    public string UserId { get; set; }

    //will do if time for FE.
    //public byte[]? Image { get; set; }

    public Data.Models.ForumPost Map()
    {
      return new Data.Models.ForumPost
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
