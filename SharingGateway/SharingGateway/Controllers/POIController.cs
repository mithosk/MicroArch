using AgileServiceBus.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharingGateway.Models;
using SharingGateway.Models.Enums;
using SharingGateway.Models.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowingStoryModels = SharingGateway.BusNamespaces.Flowing.Story.Models;
using FlowingStoryRequests = SharingGateway.BusNamespaces.Flowing.Story.Requests;

namespace SharingGateway.Controllers
{
    [Route("poi")]
    [ApiController]
    public class POIController : Controller
    {
        private readonly IGatewayBus _bus;
        private readonly ITraceScope _traceScope;

        public POIController(IGatewayBus bus, ITraceScope traceScope)
        {
            _bus = bus;
            _traceScope = traceScope;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<POI>>> List([FromQuery] POIFilter filter)
        {
            //point of interest searching
            List<FlowingStoryModels.POI> poi = await _bus.RequestAsync<List<FlowingStoryModels.POI>>(new FlowingStoryRequests.SearchPOI
            {
                Latitude = filter.Latitude,
                Longitude = filter.Longitude,
                Radius = filter.Radius
            },
            _traceScope);

            //response
            return poi
                .Select(poi => new POI
                {
                    Type = (StoryType)poi.Type,
                    Latitude = poi.Latitude,
                    Longitude = poi.Longitude,
                    Distance = poi.Distance
                })
                .ToList();
        }
    }
}