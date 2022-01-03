using AgileServiceBus.Interfaces;
using FakeItEasy;
using StoryService.Data;
using StoryService.Data.Enums;
using StoryService.Data.Interfaces;
using StoryService.Data.Models;
using StoryService.Subscribers.Responders;
using StoryService.Utilities.Logic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FlowingStoryEnums = StoryService.BusNamespaces.Flowing.Story.Enums;
using FlowingStoryModels = StoryService.BusNamespaces.Flowing.Story.Models;
using FlowingStoryRequests = StoryService.BusNamespaces.Flowing.Story.Requests;

namespace StoryService.Test.Subscribers.Responders
{
    public class SearchPOIResponderTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task UnsuccessfullyPOISearching(FlowingStoryRequests.SearchPOI message)
        {
            //geolocation utility fake
            GeolocationUtility geolocationUtility = A.Fake<GeolocationUtility>();

            //execution
            List<FlowingStoryModels.POI> response;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                SearchPOIResponder responder = new(dataContext, geolocationUtility);
                responder.TraceScope = A.Fake<ITraceScope>();
                response = (List<FlowingStoryModels.POI>)await responder.RespondAsync(message);
            }

            //check
            Assert.NotNull(response);
            Assert.Empty(response);
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task SuccessfullyPOISearching(FlowingStoryRequests.SearchPOI message)
        {
            //data context fake
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Stories.AddRangeAsync(new List<Story>
                {
                    new Story
                    {
                        Type = StoryType.Alien,
                        Title = "title",
                        Tale = "tale",
                        Latitude = (float)44.1,
                        Longitude = (float)12.2
                    },
                    new Story
                    {
                        Type = StoryType.Alien,
                        Title = "title",
                        Tale = "tale",
                        Latitude = (float)44.3,
                        Longitude = (float)12.4
                    },
                    new Story
                    {
                        Type = StoryType.Ghost,
                        Title = "title",
                        Tale = "tale",
                        Latitude = (float)44.5,
                        Longitude = (float)12.6
                    },
                    new Story
                    {
                        Type = StoryType.Ghost,
                        Title = "title",
                        Tale = "tale",
                        Latitude = (float)44.7,
                        Longitude = (float)12.8
                    },
                    new Story
                    {
                        Type = StoryType.Monster,
                        Title = "title",
                        Tale = "tale",
                        Latitude = (float)44.8,
                        Longitude = (float)12.9
                    }
                });

                await dataContext.SaveChangesAsync();
            }

            //geolocation utility fake
            GeolocationUtility geolocationUtility = A.Fake<GeolocationUtility>();
            A.CallTo(() => geolocationUtility.MinLat(A<float>.Ignored, A<ushort>.Ignored)).Returns((float)44.1);
            A.CallTo(() => geolocationUtility.MaxLat(A<float>.Ignored, A<ushort>.Ignored)).Returns((float)44.8);
            A.CallTo(() => geolocationUtility.MinLon(A<float>.Ignored, A<float>.Ignored, A<ushort>.Ignored)).Returns((float)12.2);
            A.CallTo(() => geolocationUtility.MaxLon(A<float>.Ignored, A<float>.Ignored, A<ushort>.Ignored)).Returns((float)12.8);
            A.CallTo(() => geolocationUtility.Distance(A<float>.Ignored, A<float>.Ignored, A<float>.Ignored, A<float>.Ignored)).Returns((uint)15);

            //execution
            List<FlowingStoryModels.POI> response;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                SearchPOIResponder responder = new(dataContext, geolocationUtility);
                responder.TraceScope = A.Fake<ITraceScope>();
                response = (List<FlowingStoryModels.POI>)await responder.RespondAsync(message);
            }

            //check
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.Equal(2, response.Count);
            Assert.Equal(15, (int)response[0].Distance);
            Assert.Equal(15, (int)response[1].Distance);
            Assert.True(response.Any(poi => poi.Type == FlowingStoryEnums.StoryType.Alien) == true);
            Assert.True(response.Any(poi => poi.Type == FlowingStoryEnums.StoryType.Ghost) == true);
        }

        public static List<object[]> LoadMessages()
        {
            return LoadJson<FlowingStoryRequests.SearchPOI>("Requests/SearchPOI.json");
        }
    }
}