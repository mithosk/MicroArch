using AgileServiceBus.Additionals;
using AgileServiceBus.Drivers;
using AgileServiceBus.Interfaces;
using CleanerScheduler.BusNamespaces.Flowing.Story.Events;

namespace CleanerScheduler
{
    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(() =>
            {
                ISchedulerBus sb = new RabbitMQDriver(Env.Get("RABBITMQ_CONN_STR"));





                //ObsoleteStories
                sb.Schedule("* * * * *", () =>
                {
                    return new ObsoleteStories
                    {
                        DateTo = DateTime.UtcNow.AddDays(-30)
                    };
                },
                async (Exception e) =>
                {
                    await Console.Error.WriteLineAsync("ObsoleteStories (" + e.GetType().Name + "): " + e.Message);
                });





                return sb;
            });
        }
    }
}