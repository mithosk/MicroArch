using AgileServiceBus.Interfaces;
using System;
using System.Threading.Tasks;
using UserService.BusNamespaces.Flowing.User.Requests;
using UserService.Data.Interfaces;
using UserService.Data.Models;
using UserService.Data.Repositories;
using UserService.Utilities.Logic;
using FlowingUserModels = UserService.BusNamespaces.Flowing.User.Models;

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
                user = await _dataContext.Users.FindByAsync(message.Email);

            //user not found
            if (user == null)
                return null;

            //invalid password
            if (user.PasswordHash != _passwordUtility.ToHash(message.Password))
                return null;

            //temporary key
            if (!user.AccessKey.HasValue)
                using (TraceScope.CreateSubScope("CreateAccessKey"))
                {
                    user.AccessKey = Guid.NewGuid();
                    await _dataContext.SaveChangesAsync();
                }

            //access data
            return new FlowingUserModels.Access
            {
                UserId = user.ExternalId,
                AccessKey = user.AccessKey.Value
            };
        }
    }
}