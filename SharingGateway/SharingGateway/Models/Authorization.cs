using System;

namespace SharingGateway.Models
{
    public class Authorization
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public Guid? RefreshToken { get; set; }
        public Guid? UserId { get; set; }
    }
}