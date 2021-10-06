using System;

namespace SharingGateway.BusNamespaces.Flowing.User.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ushort PublishedStories { get; set; }
        public DateTime? LastPublishDate { get; set; }
    }
}