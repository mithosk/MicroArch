using AgileServiceBus.Attributes;
using AgileServiceBus.Interfaces;
using System;

namespace SharingGateway.BusNamespaces.Flowing.Story.Requests
{
    [BusNamespace(Directory = "Flowing", Subdirectory = "Story")]
    public class StoryDetail : ICacheId
    {
        public Guid Id { get; set; }

        public string CreateCacheSuffix()
        {
            return Id.ToString();
        }
    }
}