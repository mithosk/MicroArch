using AgileServiceBus.Attributes;
using System;

namespace StoryService.BusNamespaces.Flowing.Story.Events
{
    [BusNamespace(Directory = "Flowing", Subdirectory = "Story")]
    public class PublishedStory
    {
        public Guid StoryId { get; set; }
        public DateTime PublicationDate { get; set; }
        public Guid UserId { get; set; }
    }
}