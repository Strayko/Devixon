using System.ComponentModel.DataAnnotations;
using DevixonApi.Data.Attributes;

namespace DevixonApi.Data.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [EmailUserUnique]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}