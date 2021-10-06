using AgileServiceBus.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
            //request pagination parameters
            string pageIndexHeader = Request.Headers["PageIndex"].ToString();
            string pageSizeHeader = Request.Headers["PageSize"].ToString();
            uint pageIndex = string.IsNullOrEmpty(pageIndexHeader) ? DEFAULT_PAGE_INDEX : uint.Parse(pageIndexHeader);
            ushort pageSize = string.IsNullOrEmpty(pageSizeHeader) ? DEFAULT_PAGE_SIZE : ushort.Parse(pageSizeHeader);

            //story list
            FlowingStoryModels.Stories stories = await _bus.RequestAsync<FlowingStoryModels.Stories>(new FlowingStoryRequests.StoryList
            {
                TextFilter = filter.Text,
                PageIndex = pageIndex,
                PageSize = pageSize
            },
            _traceScope);

            //response pagination parameters
            Response.Headers.Add("PageIndex", pageIndex.ToString());
            Response.Headers.Add("PageSize", pageSize.ToString());
            Response.Headers.Add("PageCount", stories.PageCount.ToString());
            Response.Headers.Add("TotalItemCount", stories.TotalItemCount.ToString());

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