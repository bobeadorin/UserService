using System.Text.Json.Serialization;

namespace UserService.Models
{
    public class UserLoginModel
    {
        [JsonRequired]   
        public string Email { get; set; }
        [JsonRequired]
        public string Password { get; set; }
    }
}
