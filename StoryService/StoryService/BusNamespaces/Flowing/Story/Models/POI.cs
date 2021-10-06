using StoryService.BusNamespaces.Flowing.Story.Enums;

namespace StoryService.BusNamespaces.Flowing.Story.Models
{
    public class POI
    {
        public StoryType Type { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public uint Distance { get; set; }
    }
}