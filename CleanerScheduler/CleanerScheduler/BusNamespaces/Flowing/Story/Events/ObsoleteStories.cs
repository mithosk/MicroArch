using AgileServiceBus.Attributes;

namespace CleanerScheduler.BusNamespaces.Flowing.Story.Events
{
    [BusNamespace(Directory = "Flowing", Subdirectory = "Story")]
    public class ObsoleteStories
    {
        public DateTime DateTo { get; set; }
    }
}