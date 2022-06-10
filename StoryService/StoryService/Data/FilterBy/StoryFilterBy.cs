using System;

namespace StoryService.Data.FilterBy
{
    public class StoryFilterBy
    {
        public string Text { get; set; }
        public float? MinLat { get; set; }
        public float? MaxLat { get; set; }
        public float? MinLon { get; set; }
        public float? MaxLon { get; set; }
        public DateTime? DateTo { get; set; }
    }
}