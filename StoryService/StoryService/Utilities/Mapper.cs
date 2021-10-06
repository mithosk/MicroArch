using StoryService.Data.Models;
using FlowingStoryEnums = StoryService.BusNamespaces.Flowing.Story.Enums;
using FlowingStoryModels = StoryService.BusNamespaces.Flowing.Story.Models;

namespace StoryService.Utilities
{
    public class Mapper
    {
        public static FlowingStoryModels.Story Map(Story story)
        {
            return new FlowingStoryModels.Story
            {
                Id = story.ExternalId,
                Type = (FlowingStoryEnums.StoryType)story.Type,
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