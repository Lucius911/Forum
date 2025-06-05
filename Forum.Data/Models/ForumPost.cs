using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Forum.Data.Models
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
  }
}
