using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forum.Data.Models;
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

    public DbSet<ForumPost> ForumPosts { get; set; }
  }
}
