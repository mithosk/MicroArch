using AgileServiceBus.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharingGateway.Controllers;
using SharingGateway.Models;
using SharingGateway.Models.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FlowingStoryEnums = SharingGateway.BusNamespaces.Flowing.Story.Enums;
using FlowingStoryModels = SharingGateway.BusNamespaces.Flowing.Story.Models;
using FlowingStoryRequests = SharingGateway.BusNamespaces.Flowing.Story.Requests;

namespace SharingGateway.Test.Controllers
{
    public class StoryControllerTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadBodies))]
        public async Task StoryCreation(Story body)
        {
            //test story
            FlowingStoryModels.Story story = new()
            {
                Id = Guid.NewGuid()
            };

            //bus fake
            IGatewayBus bus = A.Fake<IGatewayBus>();
            A.CallTo(() => bus.RequestAsync<FlowingStoryModels.Story>(A<FlowingStoryRequests.PublishStory>.Ignored, A<ITraceScope>.Ignored))
                .Returns(story);

            //trace scope fake
            ITraceScope traceScope = A.Fake<ITraceScope>();

            //execution
            ActionResult<Story> response = await new StoryController(bus, traceScope).Post(body);

            //check
            Assert.Null(response.Result);
            Assert.NotNull(response.Value);
            Assert.Equal(story.Id, response.Value.Id);
        }

        [Fact]
        public async Task UnsuccessfullyStoryRecovery()
        {
            //bus fake
            IGatewayBus bus = A.Fake<IGatewayBus>();
            A.CallTo(() => bus.RequestAsync<FlowingStoryModels.Story>(A<FlowingStoryRequests.StoryDetail>.Ignored, A<ITraceScope>.Ignored))
                .Returns(default(FlowingStoryModels.Story));

            //trace scope fake
            ITraceScope traceScope = A.Fake<ITraceScope>();

            //execution
            ActionResult<Story> response = await new StoryController(bus, traceScope).Get(Guid.NewGuid());

            //check
            Assert.NotNull(response.Result);
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public async Task SuccessfullyStoryRecovery()
        {
            //test story
            FlowingStoryModels.Story story = new()
            {
                Id = Guid.NewGuid(),
                Type = FlowingStoryEnums.StoryType.Murderer,
                Title = "title",
                Tale = "tale tale tale",
                Latitude = (float)12.12,
                Longitude = (float)43.43,
                PublicationDate = DateTime.UtcNow,
                UserId = Guid.NewGuid()
            };

            //bus fake
            IGatewayBus bus = A.Fake<IGatewayBus>();
            A.CallTo(() => bus.RequestAsync<FlowingStoryModels.Story>(A<FlowingStoryRequests.StoryDetail>.Ignored, A<ITraceScope>.Ignored))
                .Returns(story);

            //trace scope fake
            ITraceScope traceScope = A.Fake<ITraceScope>();

            //execution
            ActionResult<Story> response = await new StoryController(bus, traceScope).Get(story.Id);

            //check
            Assert.Null(response.Result);
            Assert.NotNull(response.Value);
            Assert.Equal(story.Id, response.Value.Id);
            Assert.Equal(story.Type.ToString(), response.Value.Type.ToString());
            Assert.Equal(story.Title, response.Value.Title);
            Assert.Equal(story.Tale, response.Value.Tale);
            Assert.Equal(story.Latitude, response.Value.Latitude);
            Assert.Equal(story.Longitude, response.Value.Longitude);
            Assert.Equal(story.PublicationDate, response.Value.PublicationDate);
            Assert.Equal(story.UserId, response.Value.UserId);
        }

        [Theory]
        [MemberData(nameof(LoadFilters))]
        public async Task StoryListRecovery(StoryFilter filter)
        {
            //test stories
            FlowingStoryModels.Stories stories = new()
            {
                Items = new List<FlowingStoryModels.Story>
                {
                    new FlowingStoryModels.Story
                    {
                        Id = Guid.NewGuid(),
                        Type = FlowingStoryEnums.StoryType.Alien,
                        Title = "title",
                        Tale = "tale tale tale",
                        Latitude = (float) 12.12,
                        Longitude = (float) 43.43,
                        PublicationDate = DateTime.UtcNow,
                        UserId = Guid.NewGuid()
                    },
                    new FlowingStoryModels.Story
                    {
                        Id = Guid.NewGuid(),
                        Type = FlowingStoryEnums.StoryType.Ghost,
                        Title = "title",
                        Tale = "tale tale tale",
                        Latitude = (float) 13.13,
                        Longitude = (float) 44.44,
                        PublicationDate = DateTime.UtcNow,
                        UserId = Guid.NewGuid()
                    }
                }
            };

            //bus fake
            IGatewayBus bus = A.Fake<IGatewayBus>();
            A.CallTo(() => bus.RequestAsync<FlowingStoryModels.Stories>(A<FlowingStoryRequests.StoryList>.Ignored, A<ITraceScope>.Ignored))
                .Returns(stories);

            //trace scope fake
            ITraceScope traceScope = A.Fake<ITraceScope>();

            //execution
            StoryController controller = new(bus, traceScope);
            controller.ControllerContext.HttpContext = A.Fake<HttpContext>();
            ActionResult<List<Story>> response = await controller.List(filter);

            //check
            Assert.Null(response.Result);
            Assert.NotNull(response.Value);
            Assert.Equal(stories.Items.Count, response.Value.Count);
            for (byte i = 0; i < response.Value.Count; i++)
            {
                Assert.Equal(stories.Items[i].Id, response.Value[i].Id);
                Assert.Equal(stories.Items[i].Type.ToString(), response.Value[i].Type.ToString());
                Assert.Equal(stories.Items[i].Title, response.Value[i].Title);
                Assert.Equal(stories.Items[i].Tale, response.Value[i].Tale);
                Assert.Equal(stories.Items[i].Latitude, response.Value[i].Latitude);
                Assert.Equal(stories.Items[i].Longitude, response.Value[i].Longitude);
                Assert.Equal(stories.Items[i].PublicationDate, response.Value[i].PublicationDate);
                Assert.Equal(stories.Items[i].UserId, response.Value[i].UserId);
            }
        }

        public static List<object[]> LoadBodies()
        {
            return LoadJson<Story>("Story.json");
        }

        public static List<object[]> LoadFilters()
        {
            return LoadJson<StoryFilter>("StoryFilter.json");
        }
    }
}