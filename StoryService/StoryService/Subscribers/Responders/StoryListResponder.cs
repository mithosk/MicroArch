using AgileServiceBus.Interfaces;
using Microsoft.EntityFrameworkCore;
using StoryService.Data.Enums;
using StoryService.Data.FilterBy;
using StoryService.Data.Interfaces;
using StoryService.Data.Models;
using StoryService.Data.Repositories;
using StoryService.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowingStoryModels = StoryService.BusNamespaces.Flowing.Story.Models;
using FlowingStoryRequests = StoryService.BusNamespaces.Flowing.Story.Requests;

namespace StoryService.Subscribers.Responders
{
    public class StoryListResponder : IResponder<FlowingStoryRequests.StoryList>
    {
        private readonly IDataContext _dataContext;

        public IMicroserviceBus Bus { get; set; }
        public ITraceScope TraceScope { get; set; }

        public StoryListResponder(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<object> RespondAsync(FlowingStoryRequests.StoryList message)
        {
            //filter setting
            StoryFilterBy filter = new()
            {
                Text = message.TextFilter
            };

            //filtering
            List<Story> stories;
            using (TraceScope.CreateSubScope("FindStories"))
            {
                uint skip = Pager.RecordSkip(message.PageIndex, message.PageSize);
                stories = await _dataContext.Stories
                    .AsNoTracking()
                    .FindByAsync(filter, (StorySortBy)message.SortType, skip, message.PageSize);
            }

            //counting
            uint totalItemCount;
            using (TraceScope.CreateSubScope("CountStories"))
                totalItemCount = await _dataContext.Stories.CountByAsync(filter);

            //mapping
            return new FlowingStoryModels.Stories
            {
                Items = stories.Select(sto => Mapper.Map(sto)).ToList(),
                PageCount = Pager.PageCount(totalItemCount, message.PageSize),
                TotalItemCount = totalItemCount
            };
        }
    }
}