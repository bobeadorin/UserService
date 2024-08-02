using System.Runtime.Serialization;

namespace UserService.Exceptions
{
    public class UserServiceException:Exception
    {
        public string ErrorCategory {  get; private set; }
        public string ErrorMessage { get; private set; }

        public UserServiceException(string message, string errorCategory, string uiErrorMessage = null)
        {
            ErrorCategory = errorCategory.ToString();
            ErrorMessage = uiErrorMessage;
        }

        protected UserServiceException(SerializationInfo info, StreamingContext context)
           : base(info, context)
        {
        }
    }

}
