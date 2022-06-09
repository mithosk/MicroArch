using AgileServiceBus.Attributes;
using System;

namespace UserService.BusNamespaces.Flowing.Story.Events
{
    [BusNamespace(Directory = "Flowing", Subdirectory = "Story")]
    public class ObsoleteStory
    {
        public Guid StoryId { get; set; }
    }
}