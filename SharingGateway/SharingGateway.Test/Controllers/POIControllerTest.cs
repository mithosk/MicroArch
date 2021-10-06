using AgileServiceBus.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using SharingGateway.Controllers;
using SharingGateway.Models;
using SharingGateway.Models.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FlowingStoryEnums = SharingGateway.BusNamespaces.Flowing.Story.Enums;
using FlowingStoryModels = SharingGateway.BusNamespaces.Flowing.Story.Models;
using FlowingStoryRequests = SharingGateway.BusNamespaces.Flowing.Story.Requests;

namespace SharingGateway.Test.Controllers
{
    public class POIControllerTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadFilters))]
        public async Task POIListRecovery(POIFilter filter)
        {
            //POI list for testing
            List<FlowingStoryModels.POI> poiList = new()
            {
                new FlowingStoryModels.POI
                {
                    Type = FlowingStoryEnums.StoryType.Ghost,
                    Latitude = (float)12.1,
                    Longitude = (float)44.2,
                    Distance = 10
                },
                new FlowingStoryModels.POI
                {
                    Type = FlowingStoryEnums.StoryType.Alien,
                    Latitude = (float)13.1,
                    Longitude = (float)43.2,
                    Distance = 27
                }
            };

            //bus fake
            IGatewayBus bus = A.Fake<IGatewayBus>();
            A.CallTo(() => bus.RequestAsync<List<FlowingStoryModels.POI>>(A<FlowingStoryRequests.SearchPOI>.Ignored, A<ITraceScope>.Ignored))
                .Returns(poiList);

            //trace scope fake
            ITraceScope traceScope = A.Fake<ITraceScope>();

            //execution
            ActionResult<List<POI>> response = await new POIController(bus, traceScope).List(filter);

            //check
            Assert.Null(response.Result);
            Assert.NotNull(response.Value);
            Assert.Equal(poiList.Count, response.Value.Count);
            for (byte i = 0; i < response.Value.Count; i++)
            {
                Assert.Equal(poiList[i].Type.ToString(), response.Value[i].Type.ToString());
                Assert.Equal(poiList[i].Latitude, response.Value[i].Latitude);
                Assert.Equal(poiList[i].Longitude, response.Value[i].Longitude);
                Assert.Equal(poiList[i].Distance, response.Value[i].Distance);
            }
        }

        public static List<object[]> LoadFilters()
        {
            return LoadJson<POIFilter>("POIFilter.json");
        }
    }
}