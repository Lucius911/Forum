using Forum.Data.Models.Forum;

namespace Forum.Data.Services.ForumService
{
  public interface IForumService
  {
    Task<List<ForumPost>> GetAllPostsAsync();
    Task<ForumPost> CreatePostAsync(ForumPost post);
    Task<bool> ToggleLikeAsync(int postId, string userId);
  }
}
