using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserRepository _userRepository;

        public UserController( IUserRepository userRepository )
        {
            _userRepository = userRepository;
        }
        [HttpGet("/")]
        public IActionResult Get()
        {
            return Ok("it works");
        }


        [HttpGet("/getUserById/{id}")] 
        public IActionResult GetUserById([FromRoute] int id) {
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
        public IActionResult Login([FromBody]UserLoginModel user) {

            if (_userRepository.Login(user)) {
                return Ok(); 
            }
            return BadRequest();
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
