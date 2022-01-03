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
using FlowingUserModels = UserService.BusNamespaces.Flowing.User.Models;
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
            FlowingUserModels.Access access;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                LoginResponder responder = new(dataContext, passwordUtility);
                responder.TraceScope = A.Fake<ITraceScope>();
                access = (FlowingUserModels.Access)await responder.RespondAsync(message);
            }

            //check
            Assert.Null(access);
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
                    PasswordHash = "12345",
                    Name = "name",
                    Surname = "surname"
                });

                await dataContext.SaveChangesAsync();
            }

            //password utility fake
            PasswordUtility passwordUtility = A.Fake<PasswordUtility>();
            A.CallTo(() => passwordUtility.ToHash(A<string>.Ignored))
                .Returns("ABCDE");

            //execution
            FlowingUserModels.Access access;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                LoginResponder responder = new(dataContext, passwordUtility);
                responder.TraceScope = A.Fake<ITraceScope>();
                access = (FlowingUserModels.Access)await responder.RespondAsync(message);
            }

            //check
            Assert.Null(access);
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task NewAccessKeyAuthentication(FlowingUserRequests.Login message)
        {
            //data context fake
            Guid userExternalId = Guid.NewGuid();
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Users.AddAsync(new User
                {
                    ExternalId = userExternalId,
                    Email = message.Email,
                    PasswordHash = "12345",
                    AccessKey = null,
                    Name = "name",
                    Surname = "surname"
                });

                await dataContext.SaveChangesAsync();
            }

            //password utility fake
            PasswordUtility passwordUtility = A.Fake<PasswordUtility>();
            A.CallTo(() => passwordUtility.ToHash(A<string>.Ignored))
                .Returns("12345");

            //execution
            FlowingUserModels.Access access;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                LoginResponder responder = new(dataContext, passwordUtility);
                responder.TraceScope = A.Fake<ITraceScope>();
                access = (FlowingUserModels.Access)await responder.RespondAsync(message);
            }

            //check
            Assert.Equal(userExternalId, access.UserId);
            Assert.NotEqual(Guid.Empty, access.AccessKey);
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task OldAccessKeyAuthentication(FlowingUserRequests.Login message)
        {
            //data context fake
            Guid userExternalId = Guid.NewGuid();
            Guid accessKey = Guid.NewGuid();
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Users.AddAsync(new User
                {
                    ExternalId = userExternalId,
                    Email = message.Email,
                    PasswordHash = "12345",
                    AccessKey = accessKey,
                    Name = "name",
                    Surname = "surname"
                });

                await dataContext.SaveChangesAsync();
            }

            //password utility fake
            PasswordUtility passwordUtility = A.Fake<PasswordUtility>();
            A.CallTo(() => passwordUtility.ToHash(A<string>.Ignored))
                .Returns("12345");

            //execution
            FlowingUserModels.Access access;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                LoginResponder responder = new(dataContext, passwordUtility);
                responder.TraceScope = A.Fake<ITraceScope>();
                access = (FlowingUserModels.Access)await responder.RespondAsync(message);
            }

            //check
            Assert.Equal(userExternalId, access.UserId);
            Assert.Equal(accessKey, access.AccessKey);
        }

        public static List<object[]> LoadMessages()
        {
            return LoadJson<FlowingUserRequests.Login>("Requests/Login.json");
        }
    }
}