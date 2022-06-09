using AgileServiceBus.Interfaces;
using System.Threading.Tasks;
using UserService.BusNamespaces.Flowing.Story.Events;
using UserService.Data.Interfaces;
using UserService.Data.Models;
using UserService.Data.Repositories;

namespace UserService.Subscribers.EventHandlers
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

            //delete story
            if (story != null)
                using (TraceScope.CreateSubScope("SaveChanges"))
                {
                    _dataContext.Stories.Remove(story);
                    await _dataContext.SaveChangesAsync();
                }
        }
    }
}