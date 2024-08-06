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

        public UserController( IUserRepository userRepository ,IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
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

            var accessToken = TokenUtility.GenerateAccessToken(userData, _config.GetSection("JwtToken")?.GetSection("SecretKey")?.Value);
            var refreshToken = TokenUtility.GenerateRefreshToken();

            _userRepository.SaveRefreshToken(refreshToken, userData.Id);

            var response = new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return Ok(response);
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

        [HttpPost("refresh")]
        public IActionResult Refresh(TokenResponse tokenResponse)
        {
            var userId = TokenUtility.RetriveDataFromToken(tokenResponse.AccessToken);

            var storedRefreshToken = _userRepository.GetRefreshToken(userId);

            if (storedRefreshToken != tokenResponse.RefreshToken)
              return Unauthorized();

            var newAccessToken = TokenUtility.GenerateAccessTokenFromRefreshToken(tokenResponse.RefreshToken, _config["JwtToken:SecretKey"]);

            var response = new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = tokenResponse.RefreshToken 
            };

            return Ok(response);
        }
    }
}
