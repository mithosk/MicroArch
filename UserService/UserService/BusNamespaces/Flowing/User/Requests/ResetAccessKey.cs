using AgileServiceBus.Attributes;
using System;

namespace UserService.BusNamespaces.Flowing.User.Requests
{
    [BusNamespace(Directory = "Flowing", Subdirectory = "User")]
    public class ResetAccessKey
    {
        public Guid UserId { get; set; }
    }
}