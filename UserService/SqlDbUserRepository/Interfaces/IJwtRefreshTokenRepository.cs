using UserService.SqlDbUserRepository.Models;

namespace UserService.SqlDbUserRepository.Interfaces
{
    public interface IJwtRefreshTokenRepository
    {
        public void SaveRefreshToken(string refreshToken, Guid id);
        public string GetRefreshToken(Guid userId);
        public TokenExpirationStatus CheckRefreshTokenExpirationStatus(Guid userId);
    }
}
