using AgileServiceBus.Attributes;

namespace UserService.BusNamespaces.Flowing.User.Requests
{
    [BusNamespace(Directory = "Flowing", Subdirectory = "User")]
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}