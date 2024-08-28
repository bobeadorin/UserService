using UserService.AuthService.Models;
using UserService.SqlDbUserRepository.Models;

namespace UserService.SqlDbUserRepository.Interfaces
{
    public interface IJwtRefreshTokenRepository
    {
        public void SaveRefreshToken(string refreshToken, Guid id, bool isTokenFromRefreshEndpoint);
        public RefreshToken GetRefreshToken(Guid userId);
        public Guid GetUserIdByRefreshToken(string refreshToken);
        public TokenExpirationStatus ValidateRefreshToken(string refreshToken);
        public void SaveRefreshTokenForService(string refreshToken, Guid id, bool isTokenFromRefreshEndpoint);
    }
}
