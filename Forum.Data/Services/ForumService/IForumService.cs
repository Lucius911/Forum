using Forum.Data.Models;

namespace Forum.Data.Services.ForumService
{
  public interface IForumService
  {
    Task<List<ForumPost>> GetAllPostsAsync();
    Task<ForumPost> CreatePostAsync(ForumPost post);
  }
}
