using AgileServiceBus.Interfaces;
using FakeItEasy;
using StoryService.Data;
using StoryService.Data.Interfaces;
using StoryService.Data.Models;
using StoryService.Subscribers.Responders;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FlowingStoryModels = StoryService.BusNamespaces.Flowing.Story.Models;
using FlowingStoryRequests = StoryService.BusNamespaces.Flowing.Story.Requests;

namespace StoryService.Test.Subscribers.Responders
{
    public class StoryListResponderTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task UnsuccessfullyStoryFiltering(FlowingStoryRequests.StoryList message)
        {
            //execution
            FlowingStoryModels.Stories response;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                StoryListResponder responder = new(dataContext);
                responder.TraceScope = A.Fake<ITraceScope>();
                response = (FlowingStoryModels.Stories)await responder.RespondAsync(message);
            }

            //check
            Assert.NotNull(response);
            Assert.NotNull(response.Items);
            Assert.Empty(response.Items);
            Assert.Equal(0, (int)response.PageCount);
            Assert.Equal(0, (int)response.TotalItemCount);
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task SuccessfullyStoryFiltering(FlowingStoryRequests.StoryList message)
        {
            //data context fake
            uint totalItemCount = (uint)(message.PageIndex * message.PageSize);
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                for (uint i = 0; i < totalItemCount; i++)
                    await dataContext.Stories.AddAsync(new Story
                    {
                        Title = "title-" + message.TextFilter ?? "" + "-title",
                        Tale = "tale-" + message.TextFilter ?? "" + "-tale",
                    });

                await dataContext.SaveChangesAsync();
            }

            //execution
            FlowingStoryModels.Stories response;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                StoryListResponder responder = new(dataContext);
                responder.TraceScope = A.Fake<ITraceScope>();
                response = (FlowingStoryModels.Stories)await responder.RespondAsync(message);
            }

            //check
            Assert.NotNull(response);
            Assert.NotNull(response.Items);
            Assert.NotEmpty(response.Items);
            Assert.True(response.PageCount > 0);
            Assert.Equal(totalItemCount, response.TotalItemCount);
        }

        public static List<object[]> LoadMessages()
        {
            return LoadJson<FlowingStoryRequests.StoryList>("Requests/StoryList.json");
        }
    }
}