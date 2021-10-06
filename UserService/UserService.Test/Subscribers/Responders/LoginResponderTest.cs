using AgileServiceBus.Interfaces;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Data.Interfaces;
using UserService.Data.Models;
using UserService.Subscribers.Responders;
using UserService.Utilities.Logic;
using Xunit;
using FlowingUserRequests = UserService.BusNamespaces.Flowing.User.Requests;

namespace UserService.Test.Subscribers.Responders
{
    public class LoginResponderTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task UserNotFoundAuthentication(FlowingUserRequests.Login message)
        {
            //password utility fake
            PasswordUtility passwordUtility = A.Fake<PasswordUtility>();

            //execution
            Guid? responseExternalId;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                LoginResponder responder = new(dataContext, passwordUtility);
                responder.TraceScope = A.Fake<ITraceScope>();
                responseExternalId = (Guid?)await responder.RespondAsync(message);
            }

            //check
            Assert.Null(responseExternalId);
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task InvalidPasswordAuthentication(FlowingUserRequests.Login message)
        {
            //data context fake
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Users.AddAsync(new User
                {
                    ExternalId = Guid.NewGuid(),
                    Email = message.Email,
                    PasswordHash = "12345"
                });

                await dataContext.SaveChangesAsync();
            }

            //password utility fake
            PasswordUtility passwordUtility = A.Fake<PasswordUtility>();
            A.CallTo(() => passwordUtility.ToHash(A<string>.Ignored))
                .Returns("ABCDE");

            //execution
            Guid? responseExternalId;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                LoginResponder responder = new(dataContext, passwordUtility);
                responder.TraceScope = A.Fake<ITraceScope>();
                responseExternalId = (Guid?)await responder.RespondAsync(message);
            }

            //check
            Assert.Null(responseExternalId);
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task SuccessfullyAuthentication(FlowingUserRequests.Login message)
        {
            //data context fake
            Guid userExternalId = Guid.NewGuid();
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Users.AddAsync(new User
                {
                    ExternalId = userExternalId,
                    Email = message.Email,
                    PasswordHash = "12345"
                });

                await dataContext.SaveChangesAsync();
            }

            //password utility fake
            PasswordUtility passwordUtility = A.Fake<PasswordUtility>();
            A.CallTo(() => passwordUtility.ToHash(A<string>.Ignored))
                .Returns("12345");

            //execution
            Guid? responseExternalId;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                LoginResponder responder = new(dataContext, passwordUtility);
                responder.TraceScope = A.Fake<ITraceScope>();
                responseExternalId = (Guid?)await responder.RespondAsync(message);
            }

            //check
            Assert.Equal(userExternalId, responseExternalId);
        }

        public static List<object[]> LoadMessages()
        {
            return LoadJson<FlowingUserRequests.Login>("Requests/Login.json");
        }
    }
}