using AgileServiceBus.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharingGateway.Extensions;
using SharingGateway.Models;
using SharingGateway.Models.Enums;
using SharingGateway.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowingStoryEnums = SharingGateway.BusNamespaces.Flowing.Story.Enums;
using FlowingStoryModels = SharingGateway.BusNamespaces.Flowing.Story.Models;
using FlowingStoryRequests = SharingGateway.BusNamespaces.Flowing.Story.Requests;

namespace SharingGateway.Controllers
{
    [ApiController]
    [Route("stories")]
    public class StoryController : Controller
    {
        private const uint DEFAULT_PAGE_INDEX = 1;
        private const ushort DEFAULT_PAGE_SIZE = 10;

        private readonly IGatewayBus _bus;
        private readonly ITraceScope _traceScope;

        public StoryController(IGatewayBus bus, ITraceScope traceScope)
        {
            _bus = bus;
            _traceScope = traceScope;
        }

        [HttpPost]
        public async Task<ActionResult<Story>> Post(Story body)
        {
            FlowingStoryModels.Story story = await _bus.RequestAsync<FlowingStoryModels.Story>(new FlowingStoryRequests.PublishStory
            {
                Type = (FlowingStoryEnums.StoryType)body.Type,
                Title = body.Title,
                Tale = body.Tale,
                Latitude = body.Latitude,
                Longitude = body.Longitude,
                UserId = body.UserId
            },
            _traceScope);

            return Map(story);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Story>> Get(Guid id)
        {
            //story detail
            FlowingStoryModels.Story story = await _bus.RequestAsync<FlowingStoryModels.Story>(new FlowingStoryRequests.StoryDetail
            {
                Id = id
            },
            _traceScope);

            //failure
            if (story == null)
                return NotFound();

            //response
            return Map(story);
        }

        [HttpGet]
        public async Task<ActionResult<List<Story>>> List([FromQuery] StoryFilter filter)
        {
            //request sort type parameter
            SortType sortType = Request.GetSortType<SortType>();

            //request pagination parameters
            uint pageIndex = Request.GetPageIndex() ?? DEFAULT_PAGE_INDEX;
            ushort pageSize = Request.GetPageSize() ?? DEFAULT_PAGE_SIZE;

            //story list
            FlowingStoryModels.Stories stories = await _bus.RequestAsync<FlowingStoryModels.Stories>(new FlowingStoryRequests.StoryList
            {
                TextFilter = filter.Text,
                SortType = (FlowingStoryEnums.SortType)sortType,
                PageIndex = pageIndex,
                PageSize = pageSize
            },
            _traceScope);

            //request sort type parameter
            Response.SetSortType(sortType);

            //response pagination parameters
            Response.SetPageIndex(pageIndex);
            Response.SetPageSize(pageSize);
            Response.SetPageCount(stories.PageCount);
            Response.SetTotalItemCount(stories.TotalItemCount);

            //response mapping
            return stories.Items
                .Select(sto => Map(sto))
                .ToList();
        }

        private static Story Map(FlowingStoryModels.Story story)
        {
            return new Story
            {
                Id = story.Id,
                Type = (StoryType)story.Type,
                Title = story.Title,
                Tale = story.Tale,
                Latitude = story.Latitude,
                Longitude = story.Longitude,
                PublicationDate = story.PublicationDate,
                UserId = story.UserId
            };
        }
    }
}