using AgileServiceBus.Attributes;

namespace SharingGateway.BusNamespaces.Flowing.Story.Requests
{
    [BusNamespace(Directory = "Flowing", Subdirectory = "Story")]
    public class SearchPOI
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public ushort Radius { get; set; }
    }
}