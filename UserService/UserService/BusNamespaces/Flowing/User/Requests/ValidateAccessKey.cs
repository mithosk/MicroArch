using AgileServiceBus.Attributes;
using System;

namespace UserService.BusNamespaces.Flowing.User.Requests
{
    [BusNamespace(Directory = "Flowing", Subdirectory = "User")]
    public class ValidateAccessKey
    {
        public Guid UserId { get; set; }
        public Guid AccessKey { get; set; }
    }
}