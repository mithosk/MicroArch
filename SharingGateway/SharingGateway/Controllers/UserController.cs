using AgileServiceBus.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharingGateway.Models;
using System;
using System.Threading.Tasks;
using FlowingUserModels = SharingGateway.BusNamespaces.Flowing.User.Models;
using FlowingUserRequests = SharingGateway.BusNamespaces.Flowing.User.Requests;

namespace SharingGateway.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IGatewayBus _bus;
        private readonly ITraceScope _traceScope;

        public UserController(IGatewayBus bus, ITraceScope traceScope)
        {
            _bus = bus;
            _traceScope = traceScope;
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<User>> Get(Guid id)
        {
            //user detail
            FlowingUserModels.User user = await _bus.RequestAsync<FlowingUserModels.User>(new FlowingUserRequests.UserDetail
            {
                Id = id
            },
            _traceScope);

            //failure
            if (user == null)
                return NotFound();

            //response
            return new User
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                RegistrationDate = user.RegistrationDate,
                PublishedStories = user.PublishedStories,
                LastPublishDate = user.LastPublishDate
            };
        }
    }
}