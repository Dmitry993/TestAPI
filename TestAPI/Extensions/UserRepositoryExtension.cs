using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TestAPI.Models;
using TestAPI.Shared;

namespace TestAPI.Extensions
{
    public static class UserRepositoryExtension
    {
        public static IQueryable<User> Sort(this IQueryable<User> users, string serchTerm, string propertyName, string entityName)
        {
            if (entityName.Equals("user", StringComparison.InvariantCultureIgnoreCase))
            {
               return users.Where($"user => user.{propertyName}.Contains({serchTerm})");
            }
            else if(entityName.Equals("role", StringComparison.InvariantCultureIgnoreCase))
            {
               return users.Where($"user => user.Roles.Any(role = role.{propertyName}.Contains({serchTerm})");
            }

            return users;
        }

        public async static Task<IEnumerable<User>> ToUserPageListAsync(this IQueryable<User> users, int page, int pageSize)
        {
            return await users.Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        }
    }
}
