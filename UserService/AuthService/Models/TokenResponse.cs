namespace UserService.AuthService.Models
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
