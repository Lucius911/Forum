using System.ComponentModel.DataAnnotations;

namespace Forum.Data.Models
{
  public class ForumPost
  {
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
