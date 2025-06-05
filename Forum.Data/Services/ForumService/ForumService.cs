using Forum.Data.Models.Forum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Forum.Data.Services.ForumService
{
  public class ForumService(ForumContext context, ILogger<ForumService> logger) : IForumService
  {
    private readonly ForumContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ILogger<ForumService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<List<ForumPost>> GetAllPostsAsync()
    {
      try
      {
        _logger.LogInformation("Fetching all forum posts from the database.");
        return await _context.ForumPosts.ToListAsync().ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred while fetching forum posts.");
        throw new Exception($"Error while attempting GetAllPostsAsync: {ex.Message}");
      }
    }

    public async Task<ForumPost> CreatePostAsync(ForumPost post)
    {
      try
      {
        _logger.LogInformation("Creating blog post");
        _context.ForumPosts.Add(post);
        var result = await context.SaveChangesAsync().ConfigureAwait(false);

        return post;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred while creating a forum post.");
        throw new Exception($"Error while attempting CreatePostAsync: {ex.Message}");
      }
    }

    public async Task<bool> ToggleLikeAsync(int postId, string userId)
    {
      try
      {
        var likeExists = await _context.ForumLikes.FirstOrDefaultAsync(x => x.ForumPost.Id == postId &&
                                                                      x.UserId == userId);

        //Implement facebook logic If user has already liked the post, remove the like else save the like

        if (likeExists != null)
        {
          _logger.LogInformation($"User {userId} removed like from post {postId}.");
          _context.ForumLikes.Remove(likeExists);
          await _context.SaveChangesAsync().ConfigureAwait(false);
          return false; // unliked
        }

        var like = new ForumLike
        {
          ForumPostId = postId,
          UserId = userId,
          LikedAt = DateTime.UtcNow
        };

        _context.ForumLikes.Add(like);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return true; // liked
      }
      catch (Exception e)
      {
        _logger.LogError(e, "An error occurred while liking a forum post.");
        throw new Exception($"Error while attempting ToggleLikeAsync: {e.Message}");
      }
    }
  }
}
