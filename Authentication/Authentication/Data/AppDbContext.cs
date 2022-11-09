using Microsoft.EntityFrameworkCore;
using Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Auth.Data
{
    // public DbSet<User> Users {get; set;}
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
         public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}

