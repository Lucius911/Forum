using Forum.Data;
using Forum.Data.Services.ForumService;
using Microsoft.EntityFrameworkCore;

namespace Forum;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //Add Entity Framework Core and SQL Server support
    builder.Services.AddDbContext<ForumContext>(options => 
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    //Inject Services
    builder.Services.AddScoped<IForumService,ForumService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
  }
}