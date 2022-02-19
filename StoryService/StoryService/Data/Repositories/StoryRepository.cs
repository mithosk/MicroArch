using Microsoft.EntityFrameworkCore;
using StoryService.Data.Enums;
using StoryService.Data.FilterBy;
using StoryService.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryService.Data.Repositories
{
    public static class StoryRepository
    {
        public static async Task<Story> FindByAsync(this IQueryable<Story> queryable, Guid externalId)
        {
            return await queryable
                .Where(sto => sto.ExternalId == externalId)
                .SingleOrDefaultAsync();
        }

        public static async Task<List<Story>> FindByAsync(this IQueryable<Story> queryable, StoryFilterBy filter, StorySortBy? sort, uint? skip, ushort? take)
        {
            return await QueryBy(queryable, filter, sort, skip, take)
                .ToListAsync();
        }

        public static IQueryable<Story> QueryBy(this IQueryable<Story> queryable, StoryFilterBy filter, StorySortBy? sort, uint? skip, ushort? take)
        {
            IQueryable<Story> query = queryable
               .FilterBy(filter);

            query = sort switch
            {
                StorySortBy.DateAsc => query.OrderBy(sto => sto.PublicationDate),
                StorySortBy.DateDesc => query.OrderByDescending(sto => sto.PublicationDate),
                _ => query.OrderBy(sto => sto.Id)
            };

            if (skip != null)
                query = query.Skip((int)skip.Value);

            if (take != null)
                query = query.Take(take.Value);

            return query;
        }

        public static async Task<uint> CountByAsync(this IQueryable<Story> queryable, StoryFilterBy filter)
        {
            return (uint)await queryable
                .FilterBy(filter)
                .CountAsync();
        }

        private static IQueryable<Story> FilterBy(this IQueryable<Story> query, StoryFilterBy filter)
        {
            if (filter.Text != null)
            {
                query = query
                    .Where(sto =>
                        sto.Title.Contains(filter.Text) ||
                        sto.Tale.Contains(filter.Text)
                    );
            }

            if (filter.MinLat.HasValue)
            {
                query = query
                    .Where(sto => sto.Latitude >= filter.MinLat);
            }

            if (filter.MaxLat.HasValue)
            {
                query = query
                    .Where(sto => sto.Latitude <= filter.MaxLat);
            }

            if (filter.MinLon.HasValue)
            {
                query = query
                    .Where(sto => sto.Longitude >= filter.MinLon);
            }

            if (filter.MaxLon.HasValue)
            {
                query = query
                    .Where(sto => sto.Longitude <= filter.MaxLon);
            }

            return query;
        }
    }
}