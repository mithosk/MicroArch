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
using FlowingUserModels = UserService.BusNamespaces.Flowing.User.Models;
using FlowingUserRequests = UserService.BusNamespaces.Flowing.User.Requests;

namespace UserService.Test.Subscribers.Responders
{
    public class UserDetailResponderTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task NotExistentUserRecovery(FlowingUserRequests.UserDetail message)
        {
            //execution
            FlowingUserModels.User response;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                UserDetailResponder responder = new(dataContext);
                responder.TraceScope = A.Fake<ITraceScope>();
                response = (FlowingUserModels.User)await responder.RespondAsync(message);
            }

            //check
            Assert.Null(response);
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task ExistentUserWithoutStoriesRecovery(FlowingUserRequests.UserDetail message)
        {
            //data context fake
            User user = new()
            {
                ExternalId = message.Id,
                Email = "email@email.com",
                Name = "John",
                Surname = "Doe",
                RegistrationDate = DateTime.UtcNow
            };

            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Users.AddAsync(user);
                await dataContext.SaveChangesAsync();
            }

            //execution
            FlowingUserModels.User response;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                UserDetailResponder responder = new(dataContext);
                responder.TraceScope = A.Fake<ITraceScope>();
                response = (FlowingUserModels.User)await responder.RespondAsync(message);
            }

            //check
            Assert.NotNull(response);
            Assert.Equal(user.ExternalId, response.Id);
            Assert.Equal(user.Email, response.Email);
            Assert.Equal(user.Name, response.Name);
            Assert.Equal(user.Surname, response.Surname);
            Assert.Equal(user.RegistrationDate, response.RegistrationDate);
            Assert.Equal(0, response.PublishedStories);
            Assert.Null(response.LastPublishDate);
        }

        [Theory]
        [MemberData(nameof(LoadMessages))]
        public async Task ExistentUserWithStoriesRecovery(FlowingUserRequests.UserDetail message)
        {
            //data context fake
            User user = new()
            {
                ExternalId = message.Id,
                Email = "email@email.com",
                Name = "John",
                Surname = "Doe",
                RegistrationDate = DateTime.UtcNow,
                Stories = new List<Story>
                {
                    new Story
                    {
                        PublicationDate = DateTime.MinValue
                    },
                    new Story
                    {
                        PublicationDate = DateTime.MaxValue
                    }
                }
            };

            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                await dataContext.Users.AddAsync(user);
                await dataContext.SaveChangesAsync();
            }

            //execution
            FlowingUserModels.User response;
            using (IDataContext dataContext = new DataContext(_dataOptions))
            {
                UserDetailResponder responder = new(dataContext);
                responder.TraceScope = A.Fake<ITraceScope>();
                response = (FlowingUserModels.User)await responder.RespondAsync(message);
            }

            //check
            Assert.NotNull(response);
            Assert.Equal(user.ExternalId, response.Id);
            Assert.Equal(user.Email, response.Email);
            Assert.Equal(user.Name, response.Name);
            Assert.Equal(user.Surname, response.Surname);
            Assert.Equal(user.RegistrationDate, response.RegistrationDate);
            Assert.Equal(user.Stories.Count, response.PublishedStories);
            Assert.Equal(DateTime.MaxValue, response.LastPublishDate);
        }

        public static List<object[]> LoadMessages()
        {
            return LoadJson<FlowingUserRequests.UserDetail>("Requests/UserDetail.json");
        }
    }
}