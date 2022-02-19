using AgileServiceBus.Attributes;
using StoryService.BusNamespaces.Flowing.Story.Enums;

namespace StoryService.BusNamespaces.Flowing.Story.Requests
{
    [BusNamespace(Directory = "Flowing", Subdirectory = "Story")]
    public class StoryList
    {
        public string TextFilter { get; set; }
        public SortType? SortType { get; set; }
        public uint PageIndex { get; set; }
        public ushort PageSize { get; set; }
    }
}