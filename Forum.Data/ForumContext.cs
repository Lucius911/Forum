using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forum.Data.Models.Forum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data
{
  public class ForumContext : IdentityDbContext<IdentityUser>
  {
    public ForumContext(DbContextOptions<ForumContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      // Additional model configurations can be added here

      //Not needed but good to specify relationships explicitly

      #region relationships

      modelBuilder.Entity<ForumPost>()
        .HasOne(x => x.User)
        .WithMany()
        .HasForeignKey(x => x.UserId)
        .OnDelete(DeleteBehavior.Restrict);

      #endregion

    }

    public DbSet<ForumPost> ForumPosts { get; set; }
    public DbSet<ForumComment> ForumComments { get; set; }
    public DbSet<ForumLike> ForumLikes { get; set; }
  }
}
