using UserService.Models;

namespace UserService.SqlDbUserRepository.Models
{
    public class TokenExpirationStatus
    {
        public bool IsExpired { get; set; }
        public JwtRefreshToken RefreshToken { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
