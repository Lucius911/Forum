using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Forum.Data.Models.Forum
{
  public class ForumPost
  {
    [Key]
    public int Id { get; set; }
    public string UserId { get; set; } // FK to IdentityUser

    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IdentityUser User { get; set; } //Navigation prop
    public bool IsMisleading { get; set; }

    //Collections
    public ICollection<ForumComment> Comments { get; set; }
    public ICollection<ForumLike> Likes { get; set; }

    //props not mapped
    [NotMapped]
    public int LikesCount { get; set; }
    [NotMapped]
    public string UserName { get; set; }
  }
}
