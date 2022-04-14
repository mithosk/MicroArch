using AgileServiceBus.Additionals;
using AgileServiceBus.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharingGateway.Extensions;
using SharingGateway.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using FlowingUserModels = SharingGateway.BusNamespaces.Flowing.User.Models;
using FlowingUserRequests = SharingGateway.BusNamespaces.Flowing.User.Requests;

namespace SharingGateway.Controllers
{
    [ApiController]
    [Route("authorizations")]
    public class AuthorizationController : Controller
    {
        private const byte TOKEN_EXPIRES_HOURS = 8;

        private readonly IGatewayBus _bus;
        private readonly ITraceScope _traceScope;

        public AuthorizationController(IGatewayBus bus, ITraceScope traceScope)
        {
            _bus = bus;
            _traceScope = traceScope;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Authorization>> Post(Authorization body)
        {
            //response data
            Authorization authorization = new();

            //login authentication
            if (!body.RefreshToken.HasValue)
            {
                FlowingUserModels.Access access = await _bus.RequestAsync<FlowingUserModels.Access>(new FlowingUserRequests.Login
                {
                    Email = body.Email,
                    Password = body.Password
                },
                _traceScope);

                if (access == null)
                    return Forbid();

                authorization.RefreshToken = access.AccessKey;
                authorization.UserId = access.UserId;
            }

            //refresh authentication
            if (body.RefreshToken.HasValue)
            {
                bool valid = await _bus.RequestAsync<bool>(new FlowingUserRequests.ValidateAccessKey
                {
                    UserId = body.UserId.Value,
                    AccessKey = body.RefreshToken.Value
                },
                _traceScope);

                if (!valid)
                    return Forbid();

                authorization.RefreshToken = body.RefreshToken.Value;
                authorization.UserId = body.UserId.Value;
            }

            //jwt token generation
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Expires = DateTime.UtcNow.AddHours(TOKEN_EXPIRES_HOURS),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.Get("JWT_KEY"))), SecurityAlgorithms.HmacSha256Signature),
                Audience = Env.Get("JWT_AUDIENCE"),
                Claims = new Dictionary<string, object>
                {
                    { "UserId", authorization.UserId.ToString() }
                }
            };

            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            authorization.Token = tokenHandler.WriteToken(securityToken);

            //response
            return authorization;
        }

        [HttpDelete("{userId:Guid}")]
        public async Task<ActionResult> Delete(Guid userId)
        {
            if (userId != Request.GetUserId())
                return Forbid();

            bool found = await _bus.RequestAsync<bool>(new FlowingUserRequests.ResetAccessKey
            {
                UserId = userId
            },
            _traceScope);

            if (!found)
                return NotFound();
            else
                return Ok();
        }
    }
}