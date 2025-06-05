using System.ComponentModel.DataAnnotations;

namespace Forum.Data.Models.Forum
{
  public class ForumLike
  {
    [Key]
    public int Id { get; set; }
    public int ForumPostId { get; set; } // Foreign key to ForumPost
    public ForumPost ForumPost { get; set; } // Navigation property to ForumPost
    public string UserId { get; set; } // Foreign key to User
    public DateTime LikedAt { get; set; } 
  }
}
