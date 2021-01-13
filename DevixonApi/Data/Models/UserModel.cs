using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DevixonApi.Data.Attributes;

namespace DevixonApi.Data.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [EmailUserUnique]
        public string Email { get; set; }
        public string Image { get; set; }
        [IgnoreMap]
        public string Password { get; set; }
        public DateTime TS { get; set; }
    }
}