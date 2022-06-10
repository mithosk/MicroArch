using AgileServiceBus.Interfaces;
using Microsoft.EntityFrameworkCore;
using StoryService.BusNamespaces.Flowing.Story.Events;
using StoryService.Data.Enums;
using StoryService.Data.FilterBy;
using StoryService.Data.Interfaces;
using StoryService.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryService.Subscribers.EventHandlers
{
    public class ObsoleteStoriesEventHandler : IEventHandler<ObsoleteStories>
    {
        private readonly IDataContext _dataContext;

        public IMicroserviceBus Bus { get; set; }
        public ITraceScope TraceScope { get; set; }

        public ObsoleteStoriesEventHandler(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task HandleAsync(ObsoleteStories message)
        {
            //filter setting
            StoryFilterBy filter = new()
            {
                DateTo = message.DateTo
            };

            //find stories
            List<Guid> externalIds;
            using (TraceScope.CreateSubScope("FindStories"))
            {
                externalIds = await _dataContext.Stories.QueryBy(filter, StorySortBy.DateAsc, null, null)
                    .AsNoTracking()
                    .Select(sto => sto.ExternalId)
                    .ToListAsync();
            }

            //messaging
            foreach (Guid externalId in externalIds)
                await Bus.NotifyAsync(new ObsoleteStory
                {
                    StoryId = externalId
                });
        }
    }
}