using AgileServiceBus.Additionals;
using AgileServiceBus.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharingGateway.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using FlowingUserRequests = SharingGateway.BusNamespaces.Flowing.User.Requests;

namespace SharingGateway.Controllers
{
    [ApiController]
    [Route("authorizations")]
    public class AuthorizationController : Controller
    {
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
            //authentication
            Guid? userId = await _bus.RequestAsync<Guid?>(new FlowingUserRequests.Login
            {
                Email = body.Email,
                Password = body.Password
            },
            _traceScope);

            //failure
            if (!userId.HasValue)
                return Forbid();

            //jwt token generation
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Env.Get("JWT_KEY"))), SecurityAlgorithms.HmacSha256Signature),
                Audience = Env.Get("JWT_AUDIENCE")
            };

            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);

            //response
            return new Authorization
            {
                Token = token,
                UserId = userId
            };
        }
    }
}