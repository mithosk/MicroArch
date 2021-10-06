using AgileServiceBus.Interfaces;
using Microsoft.EntityFrameworkCore;
using StoryService.BusNamespaces.Flowing.Story.Requests;
using StoryService.Data.Interfaces;
using StoryService.Data.Models;
using StoryService.Data.Repositories;
using StoryService.Utilities;
using System.Threading.Tasks;

namespace StoryService.Subscribers.Responders
{
    public class StoryDetailResponder : IResponder<StoryDetail>
    {
        private readonly IDataContext _dataContext;

        public IMicroserviceBus Bus { get; set; }
        public ITraceScope TraceScope { get; set; }

        public StoryDetailResponder(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<object> RespondAsync(StoryDetail message)
        {
            //find story
            Story story;
            using (TraceScope.CreateSubScope("FindStory"))
            {
                story = await _dataContext.Stories
                    .AsNoTracking()
                    .FindByAsync(message.Id);
            }

            //story not found
            if (story == null)
                return null;

            //response mapping
            return Mapper.Map(story);
        }
    }
}