using Forum.Data.Models;
using Forum.Data.Services.ForumService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ForumController(ILogger<ForumController> logger, IForumService forumService) : ControllerBase
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
        return this.Ok(await forumService.GetAllPostsAsync());
      }
      catch (Exception ex)
      {
        return this.BadRequest(ex.Message);
      }
    }
  }
}
