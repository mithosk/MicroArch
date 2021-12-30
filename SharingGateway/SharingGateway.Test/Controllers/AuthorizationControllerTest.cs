using AgileServiceBus.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharingGateway.Controllers;
using SharingGateway.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Xunit;
using FlowingUserModels = SharingGateway.BusNamespaces.Flowing.User.Models;
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

            A.CallTo(() => bus.RequestAsync<FlowingUserModels.Access>(A<FlowingUserRequests.Login>.Ignored, A<ITraceScope>.Ignored))
                .Returns((FlowingUserModels.Access)null);

            A.CallTo(() => bus.RequestAsync<bool>(A<FlowingUserRequests.ValidateAccessKey>.Ignored, A<ITraceScope>.Ignored))
                .Returns(false);

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
            Guid refreshToken = body.RefreshToken ?? Guid.NewGuid();
            Guid userId = body.UserId ?? Guid.NewGuid();

            //bus fake
            IGatewayBus bus = A.Fake<IGatewayBus>();

            A.CallTo(() => bus.RequestAsync<FlowingUserModels.Access>(A<FlowingUserRequests.Login>.Ignored, A<ITraceScope>.Ignored))
                .Returns(new FlowingUserModels.Access
                {
                    UserId = userId,
                    AccessKey = refreshToken
                });

            A.CallTo(() => bus.RequestAsync<bool>(A<FlowingUserRequests.ValidateAccessKey>.Ignored, A<ITraceScope>.Ignored))
                .Returns(true);

            //trace scope fake
            ITraceScope traceScope = A.Fake<ITraceScope>();

            //execution
            ActionResult<Authorization> response = await new AuthorizationController(bus, traceScope).Post(body);

            //check
            Assert.Null(response.Result);
            Assert.Null(response.Value.Email);
            Assert.Null(response.Value.Password);
            Assert.NotNull(response.Value.Token);
            Assert.Equal(refreshToken, response.Value.RefreshToken);
            Assert.Equal(userId, response.Value.UserId);
        }

        [Fact]
        public async Task RefreshTokenReset()
        {
            Guid userId = Guid.NewGuid();

            //bus fake
            IGatewayBus bus = A.Fake<IGatewayBus>();
            A.CallTo(() => bus.RequestAsync<bool>(A<FlowingUserRequests.ResetAccessKey>.Ignored, A<ITraceScope>.Ignored))
                .Returns(true);

            //trace scope fake
            ITraceScope traceScope = A.Fake<ITraceScope>();

            //jwt token fake
            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken securityToken = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Claims = new Dictionary<string, object>
                {
                    { "UserId", userId.ToString() }
                }
            });

            string token = tokenHandler.WriteToken(securityToken);

            //execution
            AuthorizationController controller = new(bus, traceScope);
            AddRequestHeader(controller, "Authorization", $"Bearer {token}");
            ActionResult response = await controller.Delete(userId);

            //check
            Assert.IsType<OkResult>(response);
        }

        public static List<object[]> LoadBodies()
        {
            return LoadJson<Authorization>("Authorization.json");
        }
    }
}