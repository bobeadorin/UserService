using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.SqlDbUserRepository.Interfaces;
using UserService.AuthService;
using UserService.AuthService.Models;
using UserService.Constant;
using UserService.Exceptions;
using Azure.Core;
using AccessToken = UserService.AuthService.Models.AccessToken;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevAuthController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly IJwtRefreshTokenRepository _jwtRefreshTokenRepository;
        private readonly IServiceLoginRepository _serviceLoginRepository;

        public DevAuthController(IConfiguration config, IJwtRefreshTokenRepository jwtRefreshTokenRepository, IServiceLoginRepository serviceLoginRepository)
        {
            _config = config;
            _jwtRefreshTokenRepository = jwtRefreshTokenRepository;
            _serviceLoginRepository = serviceLoginRepository;
        }


        [HttpPost("/serviceLogin")]
        public IActionResult ServiceLogin([FromBody] ServiceLogin serviceLoginInfo)
        {
            try
            {
                var isServiceValid = _serviceLoginRepository.AuthenticateServiceLogin(serviceLoginInfo.Username, serviceLoginInfo.Password);

                var serviceUser = new User
                {
                    Id = isServiceValid.Id,
                    Username = isServiceValid.Username,
                    Password = isServiceValid.Password
                };

                var accessToken = TokenUtility.GenerateAccessToken(serviceUser.Id, _config["JwtToken:SecretKey"]);
                var refreshToken = TokenUtility.GenerateRefreshToken();

                _jwtRefreshTokenRepository.SaveRefreshTokenForService(refreshToken, serviceUser.Id, false);

                var refreshTokenData = _jwtRefreshTokenRepository.GetRefreshToken(serviceUser.Id);

                if (string.IsNullOrEmpty(refreshTokenData.Token))
                {
                    return NotFound(new ServiceNotFoundResponse { Message = ErrorMessages.ServiceAcoountNotFound });
                }

                return Ok(new TokenResponse { AccessToken = accessToken, RefreshToken = refreshTokenData });
            }
            catch (Exception)
            {
                return NotFound(new ServiceNotFoundResponse { Message = ErrorMessages.ServiceAcoountNotFound });
            }
        }

        [HttpPost("/serviceRefresh")]
        public IActionResult ServiceRefresh([FromBody] RefreshToken refreshToken)
        {

            if (string.IsNullOrEmpty(refreshToken.Token)) return BadRequest();
            
            var tokenValidationStatus = _jwtRefreshTokenRepository.ValidateRefreshToken(refreshToken.Token);

            if (tokenValidationStatus.IsExpired  || tokenValidationStatus.NotFound) return NotFound();
            try
            {
                var userId = _jwtRefreshTokenRepository.GetUserIdByRefreshToken(refreshToken.Token);

                var newAccessToken = TokenUtility.GenerateAccessToken(userId, _config["JwtToken:SecretKey"]);
                
                return Ok(new AccessToken { Token = newAccessToken });

            }
            catch (Exception ex){ 
            
                return NotFound(ex.Message);
            }
           


        }
    }

}

