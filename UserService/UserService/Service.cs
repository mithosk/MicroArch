using AgileServiceBus.Additionals;
using AgileServiceBus.Drivers;
using AgileServiceBus.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using UserService.BusNamespaces.Flowing.Story.Events;
using UserService.BusNamespaces.Flowing.User.Requests;
using UserService.Data;
using UserService.Data.Interfaces;
using UserService.Subscribers.EventHandlers;
using UserService.Subscribers.Responders;
using UserService.Utilities.Logic;
using UserService.Validators.Events;
using UserService.Validators.Requests;

namespace UserService
{
    public class Service
    {
        private IMicroserviceLifetime _ml;

        public void Start()
        {
            _ml = new RabbitMQDriver(Env.Get("RABBITMQ_CONN_STR"));





            //event handlers
            _ml.Subscribe<PublishedStoryEventHandler, PublishedStory>(null, new PublishedStoryValidator(), null, null);





            //responders
            _ml.Subscribe<LoginResponder, Login>(new LoginValidator());
            _ml.Subscribe<UserDetailResponder, UserDetail>(new UserDetailValidator());
            _ml.Subscribe<ValidateAccessKeyResponder, ValidateAccessKey>(new ValidateAccessKeyValidator());





            //dependency injection
            _ml.Injection.AddDbContext<DataContext>();
            _ml.Injection.AddScoped<IDataContext>(spe => spe.GetService<DataContext>());
            _ml.Injection.AddScoped<INoSaveDataContext>(spe => spe.GetService<DataContext>());

            _ml.Injection.AddScoped<PasswordUtility>();





            _ml.Startup();
        }

        public void Stop()
        {
            _ml.Dispose();
        }
    }
}