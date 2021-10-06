using AgileServiceBus.Interfaces;
using System.Threading.Tasks;
using UserService.BusNamespaces.Flowing.Story.Events;
using UserService.Data.Interfaces;
using UserService.Data.Models;
using UserService.Data.Repositories;
using UserService.Exceptions;

namespace UserService.Subscribers.EventHandlers
{
    public class PublishedStoryEventHandler : IEventHandler<PublishedStory>
    {
        private readonly IDataContext _dataContext;

        public IMicroserviceBus Bus { get; set; }
        public ITraceScope TraceScope { get; set; }

        public PublishedStoryEventHandler(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task HandleAsync(PublishedStory message)
        {
            //find user
            User user;
            using (TraceScope.CreateSubScope("FindUser"))
                user = await _dataContext.Users.FindByAsync(message.UserId);

            //user not found
            if (user == null)
                throw new ObjectNotFoundException("User not found");

            //add story
            user.Stories.Add(new Story
            {
                ExternalId = message.StoryId,
                PublicationDate = message.PublicationDate
            });

            //save
            using (TraceScope.CreateSubScope("SaveChanges"))
                await _dataContext.SaveChangesAsync();
        }
    }
}