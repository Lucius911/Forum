﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Forum.Data.Models.Forum
{
  public class ForumComment
  {
    [Key]
    public int Id { get; set; }
    public int ForumPostId { get; set; } //Foreign key to ForumPost
    
    [JsonIgnore]
    public ForumPost ForumPost { get; set; } //Navigation property

    public IdentityUser User { get; set; }
    public string UserId { get; set; } //Foreign key to User
    public string CommentContent { get; set; } //Content of the comment
    public DateTime CreatedAt { get; set; } 
  }
}
