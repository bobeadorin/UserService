using System.Text.Json.Serialization;

namespace UserService.Models
{
    public class ServiceLogin
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
