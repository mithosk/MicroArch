using AgileServiceBus.Interfaces;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using StoryService.Data;
using StoryService.Data.Interfaces;
using StoryService.Data.Models;
using StoryService.Subscribers.Responders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FlowingStoryEvents = StoryService.BusNamespaces.Flowing.Story.Events;
using FlowingStoryModels = StoryService.BusNamespaces.Flowing.Story.Models;
using FlowingStoryRequests = StoryService.BusNamespaces.Flowing.Story.Requests;

namespace StoryService.Test.Subscribers.Responders
{
    public class PublishStoryResponderTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task StoryCreation(FlowingStoryRequests.PublishStory message)
        {
            //execution
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                PublishStoryResponder responder = new(dataContext);
                responder.Bus = A.Fake<IMicroserviceBus>();
                responder.TraceScope = A.Fake<ITraceScope>();
                await responder.RespondAsync(message);
            }

            //check
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                Story story = await dataContext.Stories
                    .Where(sto => sto.UserId == message.UserId)
                    .SingleOrDefaultAsync();

                Assert.NotNull(story);
                Assert.Equal(message.Type.ToString(), story.Type.ToString());
                Assert.Equal(message.Title, story.Title);
                Assert.Equal(message.Tale, story.Tale);
                Assert.Equal(message.Latitude, story.Latitude);
                Assert.Equal(message.Longitude, story.Longitude);
            }
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task NewStoryMapping(FlowingStoryRequests.PublishStory message)
        {
            //execution
            FlowingStoryModels.Story response;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                PublishStoryResponder responder = new(dataContext);
                responder.Bus = A.Fake<IMicroserviceBus>();
                responder.TraceScope = A.Fake<ITraceScope>();
                response = (FlowingStoryModels.Story)await responder.RespondAsync(message);
            }

            //check
            Assert.NotNull(response);
            Assert.Equal(message.Type, response.Type);
            Assert.Equal(message.Title, response.Title);
            Assert.Equal(message.Tale, response.Tale);
            Assert.Equal(message.Latitude, response.Latitude);
            Assert.Equal(message.Longitude, response.Longitude);
            Assert.Equal(message.UserId, response.UserId);
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task PublishedStoryNotification(FlowingStoryRequests.PublishStory message)
        {
            //bus fake
            IMicroserviceBus bus = A.Fake<IMicroserviceBus>();

            //execution
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                PublishStoryResponder responder = new(dataContext);
                responder.Bus = bus;
                responder.TraceScope = A.Fake<ITraceScope>();
                await responder.RespondAsync(message);
            }

            //check
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                Story story = await dataContext.Stories
                    .Where(sto => sto.UserId == message.UserId)
                    .SingleAsync();

                FlowingStoryEvents.PublishedStory publishedStory = GetNotifyMessage<FlowingStoryEvents.PublishedStory>(bus);
                Assert.NotNull(publishedStory);
                Assert.Equal(story.ExternalId, publishedStory.StoryId);
                Assert.Equal(story.PublicationDate, publishedStory.PublicationDate);
                Assert.Equal(story.UserId, publishedStory.UserId);
            }
        }

        public static List<object[]> LoadMessages()
        {
            return LoadJson<FlowingStoryRequests.PublishStory>("Requests/PublishStory.json");
        }
    }
}