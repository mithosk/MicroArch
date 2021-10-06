using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data.Models;

namespace UserService.Data.Repositories
{
    public static class UserRepository
    {
        public static async Task<User> FindByAsync(this IQueryable<User> queryable, Guid externalId)
        {
            return await queryable
                .Where(use => use.ExternalId == externalId)
                .SingleOrDefaultAsync();
        }

        public static async Task<User> FindByAsync(this IQueryable<User> queryable, string email)
        {
            return await queryable
                .Where(use => use.Email == email)
                .SingleOrDefaultAsync();
        }
    }
}