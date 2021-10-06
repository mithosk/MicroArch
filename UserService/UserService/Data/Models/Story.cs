using System;

namespace UserService.Data.Models
{
    public class Story
    {
        public int Id { get; set; }



        public Guid ExternalId { get; set; }

        public DateTime PublicationDate { get; set; }



        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}