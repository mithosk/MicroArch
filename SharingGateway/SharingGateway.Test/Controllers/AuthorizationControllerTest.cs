using AgileServiceBus.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using SharingGateway.Controllers;
using SharingGateway.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FlowingUserRequests = SharingGateway.BusNamespaces.Flowing.User.Requests;

namespace SharingGateway.Test.Controllers
{
    public class AuthorizationControllerTest : TestBase
    {
        [Theory]
        [MemberData(nameof(LoadBodies))]
        public async Task UnsuccessfullyAuthentication(Authorization body)
        {
            //bus fake
            IGatewayBus bus = A.Fake<IGatewayBus>();
            A.CallTo(() => bus.RequestAsync<Guid?>(A<FlowingUserRequests.Login>.Ignored, A<ITraceScope>.Ignored))
                .Returns((Guid?)null);

            //trace scope fake
            ITraceScope traceScope = A.Fake<ITraceScope>();

            //execution
            ActionResult<Authorization> response = await new AuthorizationController(bus, traceScope).Post(body);

            //check
            Assert.NotNull(response.Result);
            Assert.IsType<ForbidResult>(response.Result);
        }

        [Theory]
        [MemberData(nameof(LoadBodies))]
        public async Task SuccessfullyAuthentication(Authorization body)
        {
            //bus fake
            Guid userId = Guid.NewGuid();
            IGatewayBus bus = A.Fake<IGatewayBus>();
            A.CallTo(() => bus.RequestAsync<Guid?>(A<FlowingUserRequests.Login>.Ignored, A<ITraceScope>.Ignored))
                .Returns(userId);

            //trace scope fake
            ITraceScope traceScope = A.Fake<ITraceScope>();

            //execution
            ActionResult<Authorization> response = await new AuthorizationController(bus, traceScope).Post(body);

            //check
            Assert.Null(response.Result);
            Assert.NotNull(response.Value.Token);
            Assert.Equal(userId, response.Value.UserId);
            Assert.Null(response.Value.Email);
            Assert.Null(response.Value.Password);
        }

        public static List<object[]> LoadBodies()
        {
            return LoadJson<Authorization>("Authorization.json");
        }
    }
}