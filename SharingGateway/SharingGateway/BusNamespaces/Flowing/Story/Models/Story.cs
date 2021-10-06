using SharingGateway.BusNamespaces.Flowing.Story.Enums;
using System;

namespace SharingGateway.BusNamespaces.Flowing.Story.Models
{
    public class Story
    {
        public Guid Id { get; set; }
        public StoryType Type { get; set; }
        public string Title { get; set; }
        public string Tale { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime PublicationDate { get; set; }
        public Guid UserId { get; set; }
    }
}