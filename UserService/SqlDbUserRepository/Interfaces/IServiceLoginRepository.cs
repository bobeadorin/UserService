using UserService.Models;

namespace UserService.SqlDbUserRepository.Interfaces
{
    public interface IServiceLoginRepository
    {
        public ServiceLogin AuthenticateServiceLogin(string username, string password);
    }
}
