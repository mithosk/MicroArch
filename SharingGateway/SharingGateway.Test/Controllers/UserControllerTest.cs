using AgileServiceBus.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using SharingGateway.Controllers;
using SharingGateway.Models;
using System;
using System.Threading.Tasks;
using Xunit;
using FlowingUserModels = SharingGateway.BusNamespaces.Flowing.User.Models;
using FlowingUserRequests = SharingGateway.BusNamespaces.Flowing.User.Requests;

namespace SharingGateway.Test.Controllers
{
    public class UserControllerTest : TestBase
    {
        [Fact]
        public async Task UnsuccessfullyUserRecovery()
        {
            //bus fake
            IGatewayBus bus = A.Fake<IGatewayBus>();
            A.CallTo(() => bus.RequestAsync<FlowingUserModels.User>(A<FlowingUserRequests.UserDetail>.Ignored, A<ITraceScope>.Ignored))
                .Returns(default(FlowingUserModels.User));

            //trace scope fake
            ITraceScope traceScope = A.Fake<ITraceScope>();

            //execution
            ActionResult<User> response = await new UserController(bus, traceScope).Get(Guid.NewGuid());

            //check
            Assert.NotNull(response.Result);
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public async Task SuccessfullyUserRecovery()
        {
            //test user
            FlowingUserModels.User user = new()
            {
                Id = Guid.NewGuid(),
                Email = "email@email.it",
                Name = "John",
                Surname = "Doe",
                RegistrationDate = DateTime.UtcNow,
                PublishedStories = 3,
                LastPublishDate = DateTime.UtcNow
            };

            //bus fake
            IGatewayBus bus = A.Fake<IGatewayBus>();
            A.CallTo(() => bus.RequestAsync<FlowingUserModels.User>(A<FlowingUserRequests.UserDetail>.Ignored, A<ITraceScope>.Ignored))
                .Returns(user);

            //trace scope fake
            ITraceScope traceScope = A.Fake<ITraceScope>();

            //execution
            ActionResult<User> response = await new UserController(bus, traceScope).Get(user.Id);

            //check
            Assert.Null(response.Result);
            Assert.Equal(user.Id, response.Value.Id);
            Assert.Equal(user.Email, response.Value.Email);
            Assert.Equal(user.Name, response.Value.Name);
            Assert.Equal(user.Surname, response.Value.Surname);
            Assert.Equal(user.RegistrationDate, response.Value.RegistrationDate);
            Assert.Equal(user.PublishedStories, response.Value.PublishedStories);
            Assert.Equal(user.LastPublishDate, response.Value.LastPublishDate);
        }
    }
}