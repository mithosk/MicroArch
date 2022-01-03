using AgileServiceBus.Interfaces;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Data.Interfaces;
using UserService.Data.Models;
using UserService.Subscribers.Responders;
using Xunit;
using FlowingUserRequests = UserService.BusNamespaces.Flowing.User.Requests;

namespace UserService.Test.Subscribers.Responders
{
    public class ValidateAccessKeyResponderTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task UserNotFoundAccessKeyValidation(FlowingUserRequests.ValidateAccessKey message)
        {
            //execution
            bool valid;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                ValidateAccessKeyResponder responder = new(dataContext);
                responder.TraceScope = A.Fake<ITraceScope>();
                valid = (bool)await responder.RespondAsync(message);
            }

            //check
            Assert.False(valid);
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task InvalidAccessKeyValidation(FlowingUserRequests.ValidateAccessKey message)
        {
            //data context fake
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Users.AddAsync(new User
                {
                    ExternalId = message.UserId,
                    Email = "email",
                    PasswordHash = "password",
                    Name = "name",
                    Surname = "surname",
                    AccessKey = Guid.NewGuid()
                });

                await dataContext.SaveChangesAsync();
            }

            //execution
            bool valid;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                ValidateAccessKeyResponder responder = new(dataContext);
                responder.TraceScope = A.Fake<ITraceScope>();
                valid = (bool)await responder.RespondAsync(message);
            }

            //check
            Assert.False(valid);
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task ValidAccessKeyValidation(FlowingUserRequests.ValidateAccessKey message)
        {
            //data context fake
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Users.AddAsync(new User
                {
                    ExternalId = message.UserId,
                    Email = "email",
                    PasswordHash = "password",
                    AccessKey = message.AccessKey,
                    Name = "name",
                    Surname = "surname",
                });

                await dataContext.SaveChangesAsync();
            }

            //execution
            bool valid;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                ValidateAccessKeyResponder responder = new(dataContext);
                responder.TraceScope = A.Fake<ITraceScope>();
                valid = (bool)await responder.RespondAsync(message);
            }

            //check
            Assert.True(valid);
        }

        public static List<object[]> LoadMessages()
        {
            return LoadJson<FlowingUserRequests.ValidateAccessKey>("Requests/ValidateAccessKey.json");
        }
    }
}