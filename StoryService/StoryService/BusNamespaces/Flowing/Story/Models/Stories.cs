using System.Collections.Generic;

namespace StoryService.BusNamespaces.Flowing.Story.Models
{
    public class Stories
    {
        public List<Story> Items { get; set; }
        public uint PageCount { get; set; }
        public uint TotalItemCount { get; set; }
    }
}