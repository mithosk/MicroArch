using AgileServiceBus.Interfaces;
using StoryService.BusNamespaces.Flowing.Story.Events;
using StoryService.BusNamespaces.Flowing.Story.Requests;
using StoryService.Data.Enums;
using StoryService.Data.Interfaces;
using StoryService.Data.Models;
using StoryService.Utilities;
using System;
using System.Threading.Tasks;

namespace StoryService.Subscribers.Responders
{
    public class PublishStoryResponder : IResponder<PublishStory>
    {
        private readonly IDataContext _dataContext;

        public IMicroserviceBus Bus { get; set; }
        public ITraceScope TraceScope { get; set; }

        public PublishStoryResponder(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<object> RespondAsync(PublishStory message)
        {
            Story story = new()
            {
                ExternalId = Guid.NewGuid(),
                Type = (StoryType)message.Type,
                Title = message.Title,
                Tale = message.Tale,
                Latitude = message.Latitude,
                Longitude = message.Longitude,
                PublicationDate = DateTime.UtcNow,
                UserId = message.UserId
            };

            using (TraceScope.CreateSubScope("SaveChanges"))
            {
                await _dataContext.Stories.AddAsync(story);
                await _dataContext.SaveChangesAsync();
            }

            await Bus.NotifyAsync(new PublishedStory
            {
                StoryId = story.ExternalId,
                PublicationDate = story.PublicationDate,
                UserId = story.UserId
            });

            return Mapper.Map(story);
        }
    }
}