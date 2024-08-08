using UserService.DbConnection;
using UserService.Models;
using UserService.SqlDbUserRepository.Interfaces;
using UserService.SqlDbUserRepository.Models;

namespace UserService.SqlDbUserRepository
{
    public class JwtRefreshTokenRepository:IJwtRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public JwtRefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }


        public  void SaveRefreshToken(string refreshToken, Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            var userTokenId = _context.jwtRefreshTokens.FirstOrDefault(u => u.UserId == id);

            if (user != null && userTokenId == null)
            {
                _context.jwtRefreshTokens.Add(new JwtRefreshToken { User = user, RefreshToken = refreshToken, CreationTime = DateTime.Now.AddDays(30), IsExpired = false ,UserId = id });
                _context.SaveChanges();
            }

            if (user != null && userTokenId != null){
                userTokenId.UserId = id;
                userTokenId.RefreshToken = refreshToken;
                userTokenId.IsExpired = false;
                _context.SaveChanges();

            }
        }

        public string GetRefreshToken(Guid userId)
        {
            var token = _context.jwtRefreshTokens.FirstOrDefault(u => u.UserId == userId);

            if (token != null && token.IsExpired == false )
            {
                return token.RefreshToken;
            }

            return String.Empty;

        }

        public TokenExpirationStatus CheckRefreshTokenExpirationStatus(Guid userId)
        {
           var refreshTokenByUserId =  _context.jwtRefreshTokens.FirstOrDefault(u => u.UserId == userId);

            if(refreshTokenByUserId != null)
            {
                if(DateTime.Now > refreshTokenByUserId.CreationTime)
                {
                    refreshTokenByUserId.IsExpired = true;
                    _context.SaveChanges();
                    return new TokenExpirationStatus { IsExpired = true };
                }
            }

            return new TokenExpirationStatus { IsExpired = false};
        }
    }
}
