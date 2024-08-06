using System.Text.Json.Serialization;

namespace UserService.Models
{
    public class UserLoginModel
    {
        [JsonRequired]   
        public string Username { get; set; }
        [JsonRequired]
        public string Password { get; set; }
    }
}
