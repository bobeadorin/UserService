using System.Text.Json.Serialization;

namespace UserService.AuthService.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }
        [JsonIgnore]
        public DateTime Expiration { get; set; }
    }
}
