using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserService.Data.Models
{
    public class User
    {
        public int Id { get; set; }



        public Guid ExternalId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(1000)]
        public string PasswordHash { get; set; }

        public Guid? AccessKey { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        public DateTime RegistrationDate { get; set; }



        public virtual ICollection<Story> Stories { get; set; }
    }
}