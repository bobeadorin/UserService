﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserService.Models
{
    public class User
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
