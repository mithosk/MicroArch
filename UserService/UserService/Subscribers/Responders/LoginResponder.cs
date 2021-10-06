using AgileServiceBus.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UserService.BusNamespaces.Flowing.User.Requests;
using UserService.Data.Interfaces;
using UserService.Data.Models;
using UserService.Data.Repositories;
using UserService.Utilities.Logic;

namespace UserService.Subscribers.Responders
{
    public class LoginResponder : IResponder<Login>
    {
        private readonly IDataContext _dataContext;
        private readonly PasswordUtility _passwordUtility;

        public IMicroserviceBus Bus { get; set; }
        public ITraceScope TraceScope { get; set; }

        public LoginResponder(IDataContext dataContext, PasswordUtility passwordUtility)
        {
            _dataContext = dataContext;
            _passwordUtility = passwordUtility;
        }

        public async Task<object> RespondAsync(Login message)
        {
            //find user
            User user;
            using (TraceScope.CreateSubScope("FindUser"))
            {
                user = await _dataContext.Users
                    .AsNoTracking()
                    .FindByAsync(message.Email);
            }

            //user not found
            if (user == null)
                return null;

            //invalid password
            if (user.PasswordHash != _passwordUtility.ToHash(message.Password))
                return null;

            //authenticated user
            return user.ExternalId;
        }
    }
}