using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Utility
{
    public class UserValidator
    {
     
        
        public bool ValidateUser(User user)
        {
            if (FieldValidator.IsPasswordValid(user.Password) &&
                FieldValidator.IsUsernameValid(user.Username) &&
                FieldValidator.IsEmailValid(user.Email))
            {
                return true;
            }
            return false;

        }
    }
}
