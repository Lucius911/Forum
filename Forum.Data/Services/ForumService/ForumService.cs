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
  }
}
