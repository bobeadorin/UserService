using Microsoft.AspNetCore.Mvc;
using UserService.DbConnection;
using UserService.Exceptions;
using UserService.Models;
using UserService.SqlDbUserRepository.Interfaces;
using UserService.Constant;
using UserService.Utility;

namespace UserService.SqlDbUserRepository
{
    public class UserRepository : IUserRepository
    {
        public readonly AppDbContext _context;


        private UserValidator _userValidator = new UserValidator();

        public UserRepository(AppDbContext context) { 
            _context = context;
        }

        public List<User> GetAllUsers()
        {
            var users = _context.Users.ToList();

            return users;
        }

        public User GetUserById(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                return user;
            }

            throw new UserServiceException(ErrorMessages.UserNotFound, "1");
        }

        public bool Login(UserLoginModel user)
        {
            var validUser = _context.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == Hashing.toSHA256(user.Password));

            if (validUser != null)
            {
                return true;
            }
            return false;
        }

        public Task<IActionResult> Logout()
        {
            throw new NotImplementedException();
        }


        public async Task<UserRegistrationState> RegisterUser(User user)
        {
            if (_userValidator.ValidateUser(user))
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

                var user2 = user;
                user.Password = Hashing.toSHA256(user2.Password);

                await _context.SaveChangesAsync();

                return new UserRegistrationState{IsRegistered = true,UserData = user};
            }

            return new UserRegistrationState{IsRegistered = false,UserData = user};  
        }


        public  User? Authenticate(string username, string password)
        {
            var user  = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == Hashing.toSHA256(password));

            return user;
        }

        public void SaveRefreshToken(string refreshToken ,Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if(user != null)
            {
                user.RefreshToken = refreshToken;
                _context.SaveChanges();
            }
        }

        public string GetRefreshToken(Guid userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null) {

                return user.RefreshToken;
            }

            return String.Empty;

        }
    }
}
