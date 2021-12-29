using AgileServiceBus.Interfaces;
using System.Threading.Tasks;
using UserService.BusNamespaces.Flowing.User.Requests;
using UserService.Data.Interfaces;
using UserService.Data.Models;
using UserService.Data.Repositories;

namespace UserService.Subscribers.Responders
{
    public class ResetAccessKeyResponder : IResponder<ResetAccessKey>
    {
        private readonly IDataContext _dataContext;

        public IMicroserviceBus Bus { get; set; }
        public ITraceScope TraceScope { get; set; }

        public ResetAccessKeyResponder(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<object> RespondAsync(ResetAccessKey message)
        {
            User user;
            using (TraceScope.CreateSubScope("FindUser"))
                user = await _dataContext.Users.FindByAsync(message.UserId);

            if (user == null)
                return false;

            using (TraceScope.CreateSubScope("ChangeAccessKey"))
            {
                user.AccessKey = null;
                await _dataContext.SaveChangesAsync();
            }

            return true;
        }
    }
}