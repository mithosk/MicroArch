using AgileServiceBus.Attributes;
using SharingGateway.BusNamespaces.Flowing.Story.Enums;

namespace SharingGateway.BusNamespaces.Flowing.Story.Requests
{
    [BusNamespace(Directory = "Flowing", Subdirectory = "Story")]
    public class StoryList
    {
        public string TextFilter { get; set; }
        public SortType SortType { get; set; }
        public uint PageIndex { get; set; }
        public ushort PageSize { get; set; }
    }
}