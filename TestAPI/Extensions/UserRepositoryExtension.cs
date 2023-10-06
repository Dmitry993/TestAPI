using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using TestAPI.Models;
using TestAPI.Shared;

namespace TestAPI.Extensions
{
    public static class UserRepositoryExtension
    {
        public static IQueryable<User> Filter(this IQueryable<User> users, string searchTerm, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return users;
            }

            var userPropertyInfo = typeof(User).GetProperty(propertyName);
            if (userPropertyInfo != null && propertyName != "Roles")
            {
                var stringQuery = userPropertyInfo.PropertyType == typeof(string)
                    ? string.Format("{0}.Contains(@0)", propertyName)
                    : string.Format("{0}.ToSting() == @0", propertyName);
                return users.Where(stringQuery, searchTerm);
            }

            if (propertyName.Contains("Role"))
            {
                var rolePropertyName = propertyName.Split(".").Last();
                var rolePropertyInfo = typeof(Role).GetProperty(rolePropertyName);
                if (rolePropertyInfo != null)
                {
                    var stringQuery = rolePropertyInfo.PropertyType == typeof(string)
                        ? string.Format("Roles.Any({0}.Contains(@0))", rolePropertyName)
                        : string.Format("Roles.Any({0}.ToString() == @0)", rolePropertyName);
                    return users.Where(stringQuery, searchTerm);
                }
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
