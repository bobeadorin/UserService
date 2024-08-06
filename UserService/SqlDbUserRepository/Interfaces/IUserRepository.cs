using Microsoft.AspNetCore.Mvc;
using UserService.Models;

namespace UserService.SqlDbUserRepository.Interfaces
{
    public interface IUserRepository
    {
        public  List<User> GetAllUsers();
        public bool Login(UserLoginModel user);
        public Task<IActionResult> Logout();
        public User GetUserById(Guid id);
        public Task<UserRegistrationState> RegisterUser(User user);
        public User? Authenticate(string username, string password);
        public void SaveRefreshToken(string refreshToken, Guid id);
        public string GetRefreshToken(Guid userId);
    }
}
