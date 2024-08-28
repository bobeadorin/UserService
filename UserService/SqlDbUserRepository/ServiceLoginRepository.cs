using UserService.DbConnection;
using UserService.Models;
using UserService.SqlDbUserRepository.Interfaces;
using UserService.Utility;


namespace UserService.SqlDbUserRepository
{
    public class ServiceLoginRepository : IServiceLoginRepository
    {
        private readonly AppDbContext _context;

        public ServiceLoginRepository(AppDbContext context)
        {
            _context = context;
        }

        public ServiceLogin AuthenticateServiceLogin(string username, string password)
        {
            var user = _context.serviceLogin.FirstOrDefault(u => u.Username == username && u.Password == Hashing.toSHA256(password));
            if(user != null)
            {
                return new ServiceLogin
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = Hashing.toSHA256(password)
                };
            }
           
            throw new Exception("Username or password is incorrect");
        }
    }
}
