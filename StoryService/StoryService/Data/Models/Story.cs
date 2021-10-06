using StoryService.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace StoryService.Data.Models
{
    public class Story
    {
        public int Id { get; set; }



        public Guid ExternalId { get; set; }

        public StoryType Type { get; set; }

        [Required]
        [MaxLength(70)]
        public string Title { get; set; }

        [Required]
        public string Tale { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public DateTime PublicationDate { get; set; }

        public Guid UserId { get; set; }
    }
}