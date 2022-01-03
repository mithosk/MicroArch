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
    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(() =>
            {
                IMicroserviceLifetime ml = new RabbitMQDriver(Env.Get("RABBITMQ_CONN_STR"));





                //event handlers
                ml.Subscribe<PublishedStoryEventHandler, PublishedStory>(null, new PublishedStoryValidator(), null, null);





                //responders
                ml.Subscribe<LoginResponder, Login>(new LoginValidator());
                ml.Subscribe<ResetAccessKeyResponder, ResetAccessKey>(new ResetAccessKeyValidator());
                ml.Subscribe<UserDetailResponder, UserDetail>(new UserDetailValidator());
                ml.Subscribe<ValidateAccessKeyResponder, ValidateAccessKey>(new ValidateAccessKeyValidator());





                //dependency injection
                ml.Injection.AddDbContext<DataContext>();
                ml.Injection.AddScoped<IDataContext>(spe => spe.GetService<DataContext>());
                ml.Injection.AddScoped<INoSaveDataContext>(spe => spe.GetService<DataContext>());

                ml.Injection.AddScoped<PasswordUtility>();





                return ml;
            });
        }
    }
}