using AgileServiceBus.Interfaces;
using FakeItEasy;
using StoryService.BusNamespaces.Flowing.Story.Events;
using StoryService.Data;
using StoryService.Data.Interfaces;
using StoryService.Data.Models;
using StoryService.Subscribers.EventHandlers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace StoryService.Test.Subscribers.EventHandlers
{
    public class ObsoleteStoriesEventHandlerTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task ObsoleteStoriesNotification(ObsoleteStories message)
        {
            //bus fake
            IMicroserviceBus bus = A.Fake<IMicroserviceBus>();

            //data context fake
            Guid notifyStoryId = Guid.NewGuid();
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Stories.AddAsync(new Story
                {
                    ExternalId = notifyStoryId,
                    Title = "title",
                    Tale = "tale",
                    PublicationDate = message.DateTo.AddDays(-1)
                });

                await dataContext.Stories.AddAsync(new Story
                {
                    ExternalId = Guid.NewGuid(),
                    Title = "title",
                    Tale = "tale",
                    PublicationDate = message.DateTo.AddDays(1)
                });

                await dataContext.SaveChangesAsync();
            }

            //execution
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                ObsoleteStoriesEventHandler hanlder = new(dataContext);
                hanlder.Bus = bus;
                hanlder.TraceScope = A.Fake<ITraceScope>();
                await hanlder.HandleAsync(message);
            }

            //check
            ObsoleteStory notifyMessage = GetNotifyMessage<ObsoleteStory>(bus);
            Assert.NotNull(notifyMessage);
            Assert.Equal(notifyStoryId, notifyMessage.StoryId);
        }

        public static List<object[]> LoadMessages()
        {
            return LoadJson<ObsoleteStories>("Events/ObsoleteStories.json");
        }
    }
}