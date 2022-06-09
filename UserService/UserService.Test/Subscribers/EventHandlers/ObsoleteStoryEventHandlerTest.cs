using AgileServiceBus.Interfaces;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Data.Interfaces;
using UserService.Data.Models;
using UserService.Subscribers.EventHandlers;
using Xunit;
using FlowingStoryEvents = UserService.BusNamespaces.Flowing.Story.Events;

namespace UserService.Test.Subscribers.EventHandlers
{
    public class ObsoleteStoryEventHandlerTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task StoryRemoval(FlowingStoryEvents.ObsoleteStory message)
        {
            //data context fake
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Stories.AddAsync(new Story
                {
                    ExternalId = message.StoryId
                });

                await dataContext.SaveChangesAsync();
            }

            //execution
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                ObsoleteStoryEventHandler hanlder = new(dataContext);
                hanlder.TraceScope = A.Fake<ITraceScope>();
                await hanlder.HandleAsync(message);
            }

            //check
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                Story story = await dataContext.Stories
                    .Where(sto => sto.ExternalId == message.StoryId)
                    .SingleOrDefaultAsync();

                Assert.Null(story);
            }
        }

        public static List<object[]> LoadMessages()
        {
            return LoadJson<FlowingStoryEvents.ObsoleteStory>("Events/ObsoleteStory.json");
        }
    }
}