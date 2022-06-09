using AgileServiceBus.Interfaces;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using StoryService.BusNamespaces.Flowing.Story.Events;
using StoryService.Data;
using StoryService.Data.Interfaces;
using StoryService.Data.Models;
using StoryService.Subscribers.EventHandlers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StoryService.Test.Subscribers.EventHandlers
{
    public class ObsoleteStoryEventHandlerTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task StoryRemoval(ObsoleteStory message)
        {
            //data context fake
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Stories.AddAsync(new Story
                {
                    ExternalId = message.StoryId,
                    Title = "title",
                    Tale = "tale"
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
            return LoadJson<ObsoleteStory>("Events/ObsoleteStory.json");
        }
    }
}