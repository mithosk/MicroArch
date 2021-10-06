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
    public class Service
    {
        private IMicroserviceLifetime _ml;

        public void Start()
        {
            _ml = new RabbitMQDriver(Env.Get("RABBITMQ_CONN_STR"));





            //responders
            _ml.Subscribe<PublishStoryResponder, PublishStory>(new PublishStoryValidator());
            _ml.Subscribe<SearchPOIResponder, SearchPOI>(new SearchPOIValidator());
            _ml.Subscribe<StoryDetailResponder, StoryDetail>(new StoryDetailValidator());
            _ml.Subscribe<StoryListResponder, StoryList>(new StoryListValidator());





            //dependency injection
            _ml.Injection.AddDbContext<DataContext>();
            _ml.Injection.AddScoped<IDataContext>(spe => spe.GetService<DataContext>());
            _ml.Injection.AddScoped<INoSaveDataContext>(spe => spe.GetService<DataContext>());

            _ml.Injection.AddScoped<GeolocationUtility>();





            _ml.Startup();
        }

        public void Stop()
        {
            _ml.Dispose();
        }
    }
}