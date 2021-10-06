using AgileServiceBus.Interfaces;
using Microsoft.EntityFrameworkCore;
using StoryService.Data.FilterBy;
using StoryService.Data.Interfaces;
using StoryService.Data.Models;
using StoryService.Data.Repositories;
using StoryService.Utilities.Logic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowingStoryEnums = StoryService.BusNamespaces.Flowing.Story.Enums;
using FlowingStoryModels = StoryService.BusNamespaces.Flowing.Story.Models;
using FlowingStoryRequests = StoryService.BusNamespaces.Flowing.Story.Requests;

namespace StoryService.Subscribers.Responders
{
    public class SearchPOIResponder : IResponder<FlowingStoryRequests.SearchPOI>
    {
        private const ushort QUERY_LIMIT = 1000;

        private readonly IDataContext _dataContext;
        private readonly GeolocationUtility _geolocationUtility;

        public IMicroserviceBus Bus { get; set; }
        public ITraceScope TraceScope { get; set; }

        public SearchPOIResponder(IDataContext dataContext, GeolocationUtility geolocationUtility)
        {
            _dataContext = dataContext;
            _geolocationUtility = geolocationUtility;
        }

        public async Task<object> RespondAsync(FlowingStoryRequests.SearchPOI message)
        {
            //filter setting
            StoryFilterBy filter = new()
            {
                MinLat = _geolocationUtility.MinLat(message.Latitude, message.Radius),
                MaxLat = _geolocationUtility.MaxLat(message.Latitude, message.Radius),
                MinLon = _geolocationUtility.MinLon(message.Latitude, message.Longitude, message.Radius),
                MaxLon = _geolocationUtility.MaxLon(message.Latitude, message.Longitude, message.Radius)
            };

            //filtering
            List<Story> stories;
            using (TraceScope.CreateSubScope("FindStories"))
            {
                stories = await _dataContext.Stories
                    .AsNoTracking()
                    .FindByAsync(filter, null, QUERY_LIMIT);
            }

            //mapping
            List<FlowingStoryModels.POI> pointOfInterest = stories
                .Select(sto => new FlowingStoryModels.POI
                {
                    Type = (FlowingStoryEnums.StoryType)sto.Type,
                    Latitude = sto.Latitude,
                    Longitude = sto.Longitude,
                    Distance = _geolocationUtility.Distance(message.Latitude, message.Longitude, sto.Latitude, sto.Longitude)
                })
                .Where(poi => poi.Distance <= message.Radius)
                .ToList();

            //grouped response
            return pointOfInterest
               .GroupBy(poi => poi.Type)
               .Select(grp => grp.OrderBy(poi => poi.Distance).First())
               .ToList();
        }
    }
}