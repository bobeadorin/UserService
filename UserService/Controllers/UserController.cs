using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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

            var accessToken = TokenUtility.GenerateAccessToken(userData.Id, _config["JwtToken:SecretKey"]);
            var refreshToken = TokenUtility.GenerateRefreshToken();

            _jwtRefreshTokenRepository.SaveRefreshToken(refreshToken, userData.Id ,false);

            var response = new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = new RefreshToken { Token = refreshToken }
            };

            Response.Cookies.Append(CookieConfig.AccessToken, accessToken, CookieTokenOptions.DevAccessTokenCookie);
            Response.Cookies.Append(CookieConfig.RefreshToken, refreshToken, CookieTokenOptions.DevRefreshTokenCookie);


            return Ok(userData);
        }


        [HttpGet("/refresh")]
        public IActionResult Refresh()
        {
            var refreshTokenFromCookie = Request.Cookies[CookieConfig.RefreshToken];

            if (string.IsNullOrEmpty(refreshTokenFromCookie)) return NotFound();
            
            var tokenValidationStatus = _jwtRefreshTokenRepository.ValidateRefreshToken(refreshTokenFromCookie);

            if (tokenValidationStatus.IsExpired  || tokenValidationStatus.NotFound ) return BadRequest();
            
            try
            {
                var userId = _jwtRefreshTokenRepository.GetUserIdByRefreshToken(refreshTokenFromCookie);

                var newAccessToken = TokenUtility.GenerateAccessToken(userId, _config["JwtToken:SecretKey"]);

                Response.Cookies.Append("AccessToken", newAccessToken, CookieTokenOptions.DevAccessTokenCookie);


                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            var accessCookie = Request.Cookies[CookieConfig.AccessToken];
            var refreshCookie = Request.Cookies[CookieConfig.RefreshToken];

            if(String.IsNullOrEmpty(accessCookie) && String.IsNullOrEmpty(refreshCookie)) return BadRequest();

            Response.Cookies.Append("AccessToken", String.Empty, CookieTokenOptions.DevAccessTokenCookieLogout);
            Response.Cookies.Append("RefreshToken", String.Empty, CookieTokenOptions.DevRefreshTokenCookieLogout);
            return Ok("logoutCompleted");
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

        [HttpGet("/getAllUsers")]
        public IActionResult GetAllUsers()
        {
            return Ok(_userRepository.GetAllUsers());
        }


    }
}
