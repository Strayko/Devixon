using System;

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
        public Image Image { get; set; }
        public DateTime TS { get; set; }
    }
}