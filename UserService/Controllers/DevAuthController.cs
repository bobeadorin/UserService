//using Microsoft.AspNetCore.Mvc;
//using UserService.Models;
//using UserService.SqlDbUserRepository.Interfaces;
//using UserService.AuthService;
//using UserService.AuthService.Models;
//using UserService.Constant;
//using UserService.Exceptions;
//using Azure.Core;

//namespace UserService.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DevAuthController : ControllerBase
//    {

//        private readonly IConfiguration _config;
//        private readonly IJwtRefreshTokenRepository _jwtRefreshTokenRepository;
//        private readonly IServiceLoginRepository _serviceLoginRepository;

//        public DevAuthController(IConfiguration config, IJwtRefreshTokenRepository jwtRefreshTokenRepository , IServiceLoginRepository serviceLoginRepository )
//        {
//            _config = config;
//            _jwtRefreshTokenRepository = jwtRefreshTokenRepository;
//            _serviceLoginRepository = serviceLoginRepository;
//        }


//        [HttpPost("/serviceLogin")]
//        public IActionResult ServiceLogin([FromBody] ServiceLogin serviceLoginInfo)
//        {
//            try
//            {
//                var isServiceValid = _serviceLoginRepository.AuthenticateServiceLogin(serviceLoginInfo.Username, serviceLoginInfo.Password);

//                var serviceUser = new User
//                {
//                    Id = isServiceValid.Id,
//                    Username = isServiceValid.Username,
//                    Password = isServiceValid.Password
//                };

//                var accessToken = TokenUtility.GenerateAccessToken(serviceUser, _config["JwtToken:SecretKey"]);
//                var refreshToken = TokenUtility.GenerateRefreshToken();
                
//                _jwtRefreshTokenRepository.SaveRefreshTokenForService(refreshToken, serviceUser.Id, false);

//                var refreshTokenData = _jwtRefreshTokenRepository.GetRefreshToken(serviceUser.Id);

//                if (string.IsNullOrEmpty(refreshTokenData.Token))
//                {
//                    return NotFound(new ServiceNotFoundResponse { Message = ErrorMessages.ServiceAcoountNotFound });
//                }

//                return Ok(new TokenResponse { AccessToken = accessToken, RefreshToken = refreshTokenData });
//            }
//            catch (Exception)
//            {
//                return NotFound(new ServiceNotFoundResponse { Message = ErrorMessages.ServiceAcoountNotFound });
//            }
//        }

//        [HttpPost("/serviceRefresh")]
//        public IActionResult ServiceRefresh([FromBody] string refreshToken)
//        {

//            if (!string.IsNullOrEmpty(refreshToken))
//            {
//                var tokenValidationStatus = _jwtRefreshTokenRepository.ValidateRefreshToken(refreshToken);

//                if (tokenValidationStatus.IsExpired == false)
//                {
//                    var userId = _jwtRefreshTokenRepository.GetUserIdByRefreshToken(refreshToken);

//                    var newRefreshToken = TokenUtility.GenerateRefreshToken();

//                    var newAccessToken = TokenUtility.GenerateAccessTokenFromRefreshToken(refreshToken, _config["JwtToken:SecretKey"], userId);

//                    _jwtRefreshTokenRepository.SaveRefreshTokenForService(newRefreshToken, userId, true);

//                    var refreshTokenData = _jwtRefreshTokenRepository.GetRefreshToken(userId);

//                    if (string.IsNullOrEmpty(refreshTokenData.Token))
//                    {
//                        return NotFound(new ServiceNotFoundResponse { Message = ErrorMessages.ServiceAcoountNotFound });
//                    }

//                    return Ok(new TokenResponse { AccessToken = newAccessToken, RefreshToken = refreshTokenData });
//                }

//                return Unauthorized();
//            }

//            return BadRequest();
//        }
//    }
    
//}

