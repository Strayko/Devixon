using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevixonApi.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public bool FacebookUser { get; set; }
        public bool Active { get; set; }
        public bool Blocked { get; set; }
        [NotMapped]
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
        public int? ImageId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}