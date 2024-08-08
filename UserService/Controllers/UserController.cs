using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using UserService.AuthService;
using UserService.AuthService.Models;
using UserService.DbConnection;
using UserService.Models;
using UserService.SqlDbUserRepository;
using UserService.SqlDbUserRepository.Interfaces;
using UserService.Utility;
using static UserService.Utility.FieldValidator;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IJwtRefreshTokenRepository _jwtRefreshTokenRepository;

        public UserController( IUserRepository userRepository ,IConfiguration config , IJwtRefreshTokenRepository jwtRefreshTokenRepository)
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

        [HttpPost("/login")]
        public IActionResult Login([FromBody] UserLoginModel user)
        {

            var userData = _userRepository.Authenticate(user.Username, user.Password);

            if (userData == null)
            {
                return Unauthorized();
            }

            var accessToken = TokenUtility.GenerateAccessToken(userData,_config.GetSection("JwtToken")?.GetSection("SecretKey")?.Value);
            var refreshToken = TokenUtility.GenerateRefreshToken();

            _jwtRefreshTokenRepository.SaveRefreshToken(refreshToken, userData.Id);

            var response = new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("jwt", response.RefreshToken, cookieOptions);
            return Ok(response.AccessToken);
        }

        //x7rqNySkeVnB9GuIdR6AWfwdqi80EBUvxLhlf7uy4M8%3D
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

        [HttpPost("/refresh")]
        public IActionResult Refresh([FromBody] AccesTokenModel reqToken)
        {
            var cookieValue = Request.Cookies["jwt"];
            
            var userId = TokenUtility.RetriveDataFromToken(reqToken.AccessToken);

            var isValid = _jwtRefreshTokenRepository.CheckRefreshTokenExpirationStatus(userId);

            var storedRefreshToken = _jwtRefreshTokenRepository.GetRefreshToken(userId);

            if (storedRefreshToken != cookieValue || isValid.IsExpired == true)
                return Unauthorized();


            var newAccessToken = TokenUtility.GenerateAccessTokenFromRefreshToken(storedRefreshToken, _config["JwtToken:SecretKey"],userId);

            var response = new TokenResponse
            {
                AccessToken = newAccessToken,
            };

            return Ok(response);
        }
    }
}
