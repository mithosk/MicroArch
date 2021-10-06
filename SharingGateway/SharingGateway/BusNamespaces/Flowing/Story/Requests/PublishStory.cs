using AgileServiceBus.Attributes;
using SharingGateway.BusNamespaces.Flowing.Story.Enums;
using System;

namespace SharingGateway.BusNamespaces.Flowing.Story.Requests
{
    [BusNamespace(Directory = "Flowing", Subdirectory = "Story")]
    public class PublishStory
    {
        public StoryType? Type { get; set; }
        public string Title { get; set; }
        public string Tale { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public Guid UserId { get; set; }
    }
}