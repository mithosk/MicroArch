using AgileServiceBus.Interfaces;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Data.Interfaces;
using UserService.Data.Models;
using UserService.Subscribers.Responders;
using Xunit;
using FlowingUserRequests = UserService.BusNamespaces.Flowing.User.Requests;

namespace UserService.Test.Subscribers.Responders
{
    public class ResetAccessKeyResponderTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task UserNotFoundAccessKeyReset(FlowingUserRequests.ResetAccessKey message)
        {
            //execution
            bool found;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                ResetAccessKeyResponder responder = new(dataContext);
                responder.TraceScope = A.Fake<ITraceScope>();
                found = (bool)await responder.RespondAsync(message);
            }

            //check
            Assert.False(found);
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task UserFoundAccessKeyReset(FlowingUserRequests.ResetAccessKey message)
        {
            //data context fake
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Users.AddAsync(new User
                {
                    ExternalId = message.UserId,
                    AccessKey = Guid.NewGuid()
                });

                await dataContext.SaveChangesAsync();
            }

            //execution
            bool found;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                ResetAccessKeyResponder responder = new(dataContext);
                responder.TraceScope = A.Fake<ITraceScope>();
                found = (bool)await responder.RespondAsync(message);
            }

            //check
            Assert.True(found);

            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                User user = dataContext.Users
                    .Where(use => use.ExternalId == message.UserId)
                    .Single();

                Assert.Null(user.AccessKey);
            }
        }

        public static List<object[]> LoadMessages()
        {
            return LoadJson<FlowingUserRequests.ResetAccessKey>("Requests/ResetAccessKey.json");
        }
    }
}