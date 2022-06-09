using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data.FilterBy;
using UserService.Data.Models;

namespace UserService.Data.Repositories
{
    public static class StoryRepository
    {
        public static async Task<Story> FindByAsync(this IQueryable<Story> queryable, Guid externalId)
        {
            return await queryable
                .Where(sto => sto.ExternalId == externalId)
                .SingleOrDefaultAsync();
        }

        public static async Task<List<Story>> FindByAsync(this IQueryable<Story> queryable, StoryFilterBy filter, ushort? take)
        {
            IQueryable<Story> query = queryable
                .FilterBy(filter)
                .OrderByDescending(sto => sto.PublicationDate);

            if (take != null)
                query = query.Take(take.Value);

            return await query.ToListAsync();
        }

        public static async Task<uint> CountByAsync(this IQueryable<Story> queryable, StoryFilterBy filter)
        {
            return (uint)await queryable
                .FilterBy(filter)
                .CountAsync();
        }

        private static IQueryable<Story> FilterBy(this IQueryable<Story> query, StoryFilterBy filter)
        {
            if (filter.UserExternalId != null)
            {
                query = query
                    .Where(sto => sto.User.ExternalId == filter.UserExternalId.Value);
            }

            return query;
        }
    }
}