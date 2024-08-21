using UserService.SqlDbUserRepository.Models;

namespace UserService.SqlDbUserRepository.Interfaces
{
    public interface IJwtRefreshTokenRepository
    {
        public void SaveRefreshToken(string refreshToken, Guid id , bool tokenFromRefresh);
        public string GetRefreshToken(Guid userId);
        public Guid GetUserIdByRefreshToken(string refreshToken);
        public TokenExpirationStatus ValidateRefreshToken(string refreshToken);
    }
}
