using Forum.Data.Services.ForumService;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Data.Extensions
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddForumDataServices(this IServiceCollection services)
    {
      services.AddScoped<IForumService, ForumService>();
      return services;
    }
  }
}
