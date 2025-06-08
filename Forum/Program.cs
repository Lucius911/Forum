using Forum.Data;
using Forum.Data.Extensions;
using Forum.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Forum;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    //Bind jwt settings
    var jwtSettings = builder.Configuration.GetSection("Jwt").Get<DTOs.JwtSettings>();
    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
      c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        Description = "JWT Auth",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
      });

      c.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
        {
          new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer"
            }
          },
          Array.Empty<string>()
        }
      });
    });

    //Add Entity Framework Core and SQL Server support
    builder.Services.AddDbContext<ForumContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    //Inject Services
    builder.Services.AddForumDataServices();
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
      .AddEntityFrameworkStores<ForumContext>()
      .AddDefaultTokenProviders();

    builder.Services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings?.Issuer,
        ValidAudience = jwtSettings?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings?.Key)),
        //be strict on validation Expiry
        ClockSkew = TimeSpan.Zero
      };
    });

    // Add authorization
    builder.Services.AddAuthorization();

    //ah classic cors add this so my FE works
    builder.Services.AddCors(options =>
    {
      options.AddPolicy(name: "AllowMyFE", policy =>
      {
        policy.WithOrigins("http://localhost:8080")
          .AllowAnyHeader()
          .AllowAnyMethod();
      });
    });
    
    var app = builder.Build();
    app.UseCors("AllowMyFE");
    app.UseAuthentication();
    app.UseAuthorization();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    //TODO: Remove this if you figure out wtf
    app.Use(async (context, next) =>
    {
      Console.WriteLine($"Incoming: {context.Request.Method} {context.Request.Path}");
      await next();
    });

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
  }
}