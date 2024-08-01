using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.DbConnection;
using UserService.Models;
using UserService.Utility;
using static UserService.Utility.FieldValidator;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("/register")]
        public async Task<IActionResult> RegisterUser([FromBody]User user)
        {

            if (FieldValidator.IsPasswordValid(user.Password) &&
                FieldValidator.IsUsernameValid(user.Username) &&
                FieldValidator.IsEmailValid(user.Email))
            {
                _context.Users.Add(new User
                {
                    Password = Hashing.toSHA256(user.Password),
                    Username = user.Username,
                    Email = user.Email,
                    Address = user.Address,
                    Country = user.Country,
                    Currency = user.Currency,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                });
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(RegisterUser), new { id = user.Id }, user);
            }
            else
            {
                return BadRequest("Bad input");
            }
        }

        [HttpGet("/getAllUsers")]
        public IActionResult GetAllUsers()
        {
           return Ok(_context.Users.Select( u => new {u.Id , u.Username , u.Email, u.Password}).ToList());
        }
    }
}
