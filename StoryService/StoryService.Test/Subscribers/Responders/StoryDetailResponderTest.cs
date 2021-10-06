using AgileServiceBus.Interfaces;
using FakeItEasy;
using StoryService.Data;
using StoryService.Data.Enums;
using StoryService.Data.Interfaces;
using StoryService.Data.Models;
using StoryService.Subscribers.Responders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FlowingStoryModels = StoryService.BusNamespaces.Flowing.Story.Models;
using FlowingStoryRequests = StoryService.BusNamespaces.Flowing.Story.Requests;

namespace StoryService.Test.Subscribers.Responders
{
    public class StoryDetailResponderTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task NotExistentStoryRecovery(FlowingStoryRequests.StoryDetail message)
        {
            //execution
            FlowingStoryModels.Story response;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                StoryDetailResponder responder = new(dataContext);
                responder.TraceScope = A.Fake<ITraceScope>();
                response = (FlowingStoryModels.Story)await responder.RespondAsync(message);
            }

            //check
            Assert.Null(response);
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task ExistentStoryRecovery(FlowingStoryRequests.StoryDetail message)
        {
            //data context fake
            Story story = new()
            {
                ExternalId = message.Id,
                Type = StoryType.Monster,
                Title = "Monster in the castle",
                Tale = "I saw a monster in the castle at night",
                Latitude = (float)43.9,
                Longitude = (float)12.6,
                PublicationDate = DateTime.UtcNow,
                UserId = Guid.NewGuid()
            };

            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Stories.AddAsync(story);
                await dataContext.SaveChangesAsync();
            }

            //execution
            FlowingStoryModels.Story response;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                StoryDetailResponder responder = new(dataContext);
                responder.TraceScope = A.Fake<ITraceScope>();
                response = (FlowingStoryModels.Story)await responder.RespondAsync(message);
            }

            //check
            Assert.NotNull(response);
            Assert.Equal(story.ExternalId, response.Id);
            Assert.Equal(story.Type.ToString(), response.Type.ToString());
            Assert.Equal(story.Title, response.Title);
            Assert.Equal(story.Tale, response.Tale);
            Assert.Equal(story.Latitude, response.Latitude);
            Assert.Equal(story.Longitude, response.Longitude);
            Assert.Equal(story.PublicationDate, response.PublicationDate);
            Assert.Equal(story.UserId, response.UserId);
        }

        public static List<object[]> LoadMessages()
        {
            return LoadJson<FlowingStoryRequests.StoryDetail>("Requests/StoryDetail.json");
        }
    }
}