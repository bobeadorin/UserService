using UserService.AuthService.Models;
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

        public void SaveRefreshToken(string refreshToken, Guid id,  bool isTokenFromRefreshEndpoint )
        { 
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            var userTokenId = _context.jwtRefreshTokens.FirstOrDefault(u => u.UserId == id);

            if (user != null && userTokenId == null)
            {
                _context.jwtRefreshTokens.Add(new JwtRefreshToken { RefreshToken = refreshToken, CreationTime = DateTime.Now, IsExpired = false ,UserId = id , ExpirationDate = DateTime.Now.AddDays(5) });
                _context.SaveChanges();
            }

            if (user != null && userTokenId != null){
                userTokenId.UserId = id;
                userTokenId.RefreshToken = refreshToken;
                userTokenId.IsExpired = false;
                userTokenId.CreationTime = DateTime.Now;
                userTokenId.ExpirationDate = isTokenFromRefreshEndpoint ? userTokenId.ExpirationDate : userTokenId.CreationTime.AddDays(5);

                _context.jwtRefreshTokens.Update(userTokenId);
                _context.SaveChanges();
            }
        }

        public void SaveRefreshTokenForService(string refreshToken, Guid id, bool isTokenFromRefreshEndpoint)
        {
            var user = _context.serviceLogin.FirstOrDefault(u => u.Id == id);
            var userTokenId = _context.jwtRefreshTokens.FirstOrDefault(u => u.UserId == id);

            if (user != null && userTokenId == null)
            {
                _context.jwtRefreshTokens.Add(new JwtRefreshToken { RefreshToken = refreshToken, CreationTime = DateTime.Now, IsExpired = false, UserId = id, ExpirationDate = DateTime.Now.AddDays(5) });
                _context.SaveChanges();
            }

            if (user != null && userTokenId != null)
            {
                userTokenId.UserId = id;
                userTokenId.RefreshToken = refreshToken;
                userTokenId.IsExpired = false;
                userTokenId.CreationTime = DateTime.Now;
                userTokenId.ExpirationDate = isTokenFromRefreshEndpoint ? userTokenId.ExpirationDate : userTokenId.CreationTime.AddDays(5);

                _context.jwtRefreshTokens.Update(userTokenId);
                _context.SaveChanges();
            }
        }


        public RefreshToken GetRefreshToken(Guid userId)
        {
            var token = _context.jwtRefreshTokens.FirstOrDefault(u => u.UserId == userId);

            if (token != null && token.IsExpired == false )
            {
                return new RefreshToken
                {
                    Token = token.RefreshToken,
                    Expiration = token.ExpirationDate
                };
            }

            return new RefreshToken
                {
                    Token = string.Empty,
                }; 
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

        public TokenExpirationStatus ValidateRefreshToken(string refreshToken)
        {
            var userRefreshToken = _context.jwtRefreshTokens.FirstOrDefault(u => u.RefreshToken == refreshToken);
            var isTokenExpired = CheckAndSetRefreshTokenExpirationStatus(refreshToken); 

            if(userRefreshToken != null && isTokenExpired == false  && refreshToken == userRefreshToken.RefreshToken)
            {
                return new TokenExpirationStatus
                {
                    IsExpired = false,
                    ExpirationDate = userRefreshToken.ExpirationDate,
                };
            }

            return  new TokenExpirationStatus
            {
                IsExpired = true,
            };
        }

        private bool CheckAndSetRefreshTokenExpirationStatus(string refreshToken)
        {
            var refreshTokenByUserId = _context.jwtRefreshTokens.FirstOrDefault(u => u.RefreshToken == refreshToken);

            if (refreshTokenByUserId != null)
            {
                if (DateTime.Now > refreshTokenByUserId.ExpirationDate)
                {
                    refreshTokenByUserId.IsExpired = true;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}
