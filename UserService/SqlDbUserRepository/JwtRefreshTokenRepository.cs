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
                _context.jwtRefreshTokens.Add(new JwtRefreshToken { User = user, RefreshToken = refreshToken, CreationTime = DateTime.Now, IsExpired = false ,UserId = id , ExpirationDate = DateTime.Now.AddDays(5) });
                _context.SaveChanges();
            }

            if (user != null && userTokenId != null){
                userTokenId.UserId = id;
                userTokenId.RefreshToken = refreshToken;
                userTokenId.IsExpired = false;
                userTokenId.CreationTime = DateTime.Now;
                userTokenId.ExpirationDate = userTokenId.CreationTime.AddDays(5);
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


        public Guid GetUserIdByRefreshToken(string refreshToken)
        {
            var userToken = _context.jwtRefreshTokens.FirstOrDefault(u => u.RefreshToken == refreshToken);

            if (userToken != null && userToken.IsExpired == false)
            {
                return userToken.UserId;
            }

            else {
                throw new Exception("Not a valid refreshToken");
            }

        }

        public TokenExpirationStatus CheckRefreshTokenExpirationStatus(string refreshToken)
        {
           var refreshTokenByUserId =  _context.jwtRefreshTokens.FirstOrDefault(u => u.RefreshToken == refreshToken);

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
