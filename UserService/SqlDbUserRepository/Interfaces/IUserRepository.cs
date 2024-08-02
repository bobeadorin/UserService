using Microsoft.AspNetCore.Mvc;
using UserService.Models;

namespace UserService.SqlDbUserRepository.Interfaces
{
    public interface IUserRepository
    {
        public  List<User> GetAllUsers();
        public bool Login(UserLoginModel user);
        public Task<IActionResult> Logout();
        public User GetUserById(int id);
        public Task<UserRegistrationState> RegisterUser(User user);

    }
}
