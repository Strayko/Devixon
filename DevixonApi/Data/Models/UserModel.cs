using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using DevixonApi.Data.Attributes;
using DevixonApi.Data.Entities;

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
        public string Email { get; set; }
        public string SetImage { get; set; }
        public Image GetImage { get; set; }
        [IgnoreMap]
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}