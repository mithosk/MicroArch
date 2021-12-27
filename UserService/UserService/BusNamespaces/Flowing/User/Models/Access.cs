using System;

namespace UserService.BusNamespaces.Flowing.User.Models
{
    public class Access
    {
        public Guid UserId { get; set; }
        public Guid AccessKey { get; set; }
    }
}