using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Utility
{
    public class UserValidator
    {
        //public ILogger _logger { get; set; }
        
        public bool ValidateUser(User user)
        {

            if (FieldValidator.IsPasswordValid(user.Password) &&
                FieldValidator.IsUsernameValid(user.Username) &&
                FieldValidator.IsEmailValid(user.Email))
            {
                return true;
            }
            //_logger.LogError(new EventId(), new Exception("FailedRegistration"),$"user registration failed at {nameof(ValidateUser)}");

            return false;

        }
    }
}
