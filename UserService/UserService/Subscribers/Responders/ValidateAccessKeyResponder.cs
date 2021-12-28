using AgileServiceBus.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UserService.BusNamespaces.Flowing.User.Requests;
using UserService.Data.Interfaces;
using UserService.Data.Models;
using UserService.Data.Repositories;

namespace UserService.Subscribers.Responders
{
    public class ValidateAccessKeyResponder : IResponder<ValidateAccessKey>
    {
        private readonly IDataContext _dataContext;

        public IMicroserviceBus Bus { get; set; }
        public ITraceScope TraceScope { get; set; }

        public ValidateAccessKeyResponder(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<object> RespondAsync(ValidateAccessKey message)
        {
            User user;
            using (TraceScope.CreateSubScope("FindUser"))
            {
                user = await _dataContext.Users
                    .AsNoTracking()
                    .FindByAsync(message.UserId);
            }

            if (user == null)
                return false;

            return user.AccessKey == message.AccessKey;
        }
    }
}