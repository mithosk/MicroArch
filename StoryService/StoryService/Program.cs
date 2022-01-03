using AgileServiceBus.Additionals;
using AgileServiceBus.Drivers;
using AgileServiceBus.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using StoryService.BusNamespaces.Flowing.Story.Requests;
using StoryService.Data;
using StoryService.Data.Interfaces;
using StoryService.Subscribers.Responders;
using StoryService.Utilities.Logic;
using StoryService.Validators.Requests;

namespace StoryService
{
    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(() =>
            {
                IMicroserviceLifetime ml = new RabbitMQDriver(Env.Get("RABBITMQ_CONN_STR"));





                //responders
                ml.Subscribe<PublishStoryResponder, PublishStory>(new PublishStoryValidator());
                ml.Subscribe<SearchPOIResponder, SearchPOI>(new SearchPOIValidator());
                ml.Subscribe<StoryDetailResponder, StoryDetail>(new StoryDetailValidator());
                ml.Subscribe<StoryListResponder, StoryList>(new StoryListValidator());





                //dependency injection
                ml.Injection.AddDbContext<DataContext>();
                ml.Injection.AddScoped<IDataContext>(spe => spe.GetService<DataContext>());
                ml.Injection.AddScoped<INoSaveDataContext>(spe => spe.GetService<DataContext>());

                ml.Injection.AddScoped<GeolocationUtility>();





                return ml;
            });
        }
    }
}