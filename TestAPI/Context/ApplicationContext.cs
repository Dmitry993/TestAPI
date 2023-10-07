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
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Admin",
                    Age = 20,
                    Email = "Admin@mail.ru",
                    Password = "123456",
                });

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "User" , UserId = 1},
                new Role { Id = 2, RoleName = "Admin" , UserId = 1 },
                new Role { Id = 3, RoleName = "Support" , UserId = 1 },
                new Role { Id = 4, RoleName = "SuperAdmin", UserId = 1 }
                );    
        }
    }
}
