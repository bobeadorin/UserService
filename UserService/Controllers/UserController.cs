using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.AuthService;
using UserService.AuthService.Models;
using UserService.Constant;
using UserService.Models;
using UserService.SqlDbUserRepository.Interfaces;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IJwtRefreshTokenRepository _jwtRefreshTokenRepository;

        public UserController(IUserRepository userRepository, IConfiguration config, IJwtRefreshTokenRepository jwtRefreshTokenRepository)
        {
            _userRepository = userRepository;
            _config = config;
            _jwtRefreshTokenRepository = jwtRefreshTokenRepository;
        }

        [HttpGet("/")]
        public IActionResult Get()
        {
            return Ok("it works");
        }

        [Authorize]
        [HttpGet("/getUserById/{id}")]
        public IActionResult GetUserById([FromRoute] Guid id) {
            try
            {
                var result = _userRepository.GetUserById(id);
                return Ok(result);
            }
            catch (Exception ex) {

                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("/getUser")]
        public IActionResult GetUser()
        {
            var accessToken = Request.Cookies[CookieConfig.AccessToken];
            
            if (accessToken == null) {
                return Unauthorized();
            }

            var userData = _userRepository.GetUserDataByToken(accessToken);

            return Ok(new UserProfileData
            {
               Username = userData.Username,
               FirstName = userData.FirstName,
               LastName = userData.LastName,    
               PostsNumber = userData.PostsNumber,
               FollowersNumber = userData.FollowersNumber,
               Likes = userData.Likes
            });
        }

        [HttpPost("/login")]
        public IActionResult Login([FromBody] UserLoginModel user)
        {

            var userData = _userRepository.Authenticate(user.Username, user.Password);

            if (userData == null)
            {
                return Unauthorized();
            }

            var accessToken = TokenUtility.GenerateAccessToken(userData, _config["JwtToken:SecretKey"]);
            var refreshToken = TokenUtility.GenerateRefreshToken();

            _jwtRefreshTokenRepository.SaveRefreshToken(refreshToken, userData.Id ,false);

            var response = new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = new RefreshToken { Token = refreshToken }
            };

            Response.Cookies.Append("RefreshToken", response.RefreshToken.Token, CookieTokenOptions.DevRefreshTokenCookie);
            Response.Cookies.Append("AccessToken", response.AccessToken, CookieTokenOptions.DevAccessTokenCookie);

            return Ok(userData);
        }

        [HttpPost("/register")]
        public async Task<IActionResult> RegisterUser([FromBody]User user)
        {

            var userRegistrationData = await _userRepository.RegisterUser(user);

            if (userRegistrationData != null && userRegistrationData.IsRegistered) { 
            
                return Ok();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("/getAllUsers")]
        public IActionResult GetAllUsers()
        {
            return Ok(_userRepository.GetAllUsers());
        }


        [HttpGet("/refresh")]
        public IActionResult Refresh()
        {
            var refreshTokenFromCookie = Request.Cookies[CookieConfig.RefreshToken];
            
            if(!string.IsNullOrEmpty(refreshTokenFromCookie))
            {
                var tokenValidationStatus = _jwtRefreshTokenRepository.ValidateRefreshToken(refreshTokenFromCookie);

                if (tokenValidationStatus.IsExpired == false)
                {

                    var userId = _jwtRefreshTokenRepository.GetUserIdByRefreshToken(refreshTokenFromCookie);

                    var newRefreshToken = TokenUtility.GenerateRefreshToken();

                    var newAccessToken = TokenUtility.GenerateAccessTokenFromRefreshToken(refreshTokenFromCookie, _config["JwtToken:SecretKey"], userId);

                    _jwtRefreshTokenRepository.SaveRefreshToken(newRefreshToken, userId, true);

                    Response.Cookies.Append(CookieConfig.AccessToken, newAccessToken, CookieTokenOptions.DevAccessTokenCookie);
                    Response.Cookies.Append(CookieConfig.RefreshToken, newRefreshToken, CookieTokenOptions.DevRefreshTokenCookie);

                    return Ok();
                }
                
                return Unauthorized();
            }

            return BadRequest();
        }
    }
}
