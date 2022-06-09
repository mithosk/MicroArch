using AgileServiceBus.Interfaces;
using StoryService.BusNamespaces.Flowing.Story.Events;
using StoryService.Data.Interfaces;
using StoryService.Data.Models;
using StoryService.Data.Repositories;
using StoryService.Exceptions;
using System.Threading.Tasks;

namespace StoryService.Subscribers.EventHandlers
{
    public class ObsoleteStoryEventHandler : IEventHandler<ObsoleteStory>
    {
        private readonly IDataContext _dataContext;

        public IMicroserviceBus Bus { get; set; }
        public ITraceScope TraceScope { get; set; }

        public ObsoleteStoryEventHandler(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task HandleAsync(ObsoleteStory message)
        {
            //find story
            Story story;
            using (TraceScope.CreateSubScope("FindStory"))
                story = await _dataContext.Stories.FindByAsync(message.StoryId);

            //story not found
            if (story == null)
                throw new ObjectNotFoundException("Story not found");

            //delete story
            using (TraceScope.CreateSubScope("SaveChanges"))
            {
                _dataContext.Stories.Remove(story);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}