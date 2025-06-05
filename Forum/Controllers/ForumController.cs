using System.Security.Claims;
using Forum.Data.Models.Forum;
using Forum.Data.Services.ForumService;
using Forum.DTOs.ForumPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ForumController(ILogger<ForumController> logger, IForumService _forumService) : ControllerBase
  {
    [AllowAnonymous]
    [HttpGet("/FetchAll")]
    [ProducesResponseType<List<ForumPost>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> FetchAll()
    {
      try
      {
        return this.Ok(await _forumService.GetAllPostsAsync());
      }
      catch (Exception ex)
      {
        return this.BadRequest(ex.Message);
      }
    }

    [Authorize]
    [HttpPost("/CreatePost")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePost([FromBody] CreateForumPostDto createForumPost)
    {
      try
      {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
          return Unauthorized("You have to be logged in to create a blog post.");
        }

        createForumPost.UserId = userId;
        var post = createForumPost.Map();

        await _forumService.CreatePostAsync(post);

        return this.Ok();
      }
      catch (Exception e)
      {
        return this.BadRequest(e.Message);
      }
    }

    [Authorize]
    [HttpPost("/ToggleLike")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ToggleLike([FromBody] int postId)
    {
      try
      {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
          return Unauthorized("You have to be logged in to like a post.");
        }

        var result = await _forumService.ToggleLikeAsync(postId, userId);

        return this.Ok(result ? "Post liked" : "Post unliked");
      }
      catch (Exception e)
      {
        return this.BadRequest(e.Message);
      }
    }
  }
}
