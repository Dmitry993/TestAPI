using Microsoft.EntityFrameworkCore;
using TestAPI.Models;

namespace TestAPI.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new { Id = 1, RoleName = "User"},
                new { Id = 2, RoleName = "Admin"},
                new { Id = 3, RoleName = "Support"},
                new { Id = 4, RoleName = "SuperAdmin"}
                );
        }
    }
}
