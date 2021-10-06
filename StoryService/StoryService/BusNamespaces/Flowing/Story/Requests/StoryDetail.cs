using AgileServiceBus.Attributes;
using System;

namespace StoryService.BusNamespaces.Flowing.Story.Requests
{
    [BusNamespace(Directory = "Flowing", Subdirectory = "Story")]
    public class StoryDetail
    {
        public Guid Id { get; set; }
    }
}