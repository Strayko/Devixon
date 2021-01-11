using System;
using AutoMapper;

namespace DevixonApi.Data.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [IgnoreMap]
        public string Password { get; set; }
        public DateTime TS { get; set; }
    }
}