using Microsoft.AspNetCore.Mvc;
using UserService.DbConnection;
using UserService.Exceptions;
using UserService.Models;
using UserService.SqlDbUserRepository.Interfaces;
using UserService.Constant;
using UserService.Utility;
using UserService.AuthService;
using System.Linq;

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

        public UserProfileVisited GetUserByUsername(string username,Guid userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            var loggedInUser = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null && loggedInUser != null)
            {
                if (loggedInUser.Following.Contains(user.Id))
                {
                    return new UserProfileVisited
                    {
                        userData = new UserProfileData
                        {
                            Id = user.Id,
                            Username = user.Username,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Likes = user.Likes,
                            PostsNumber = user.PostsNumber,
                            FollowersNumber = user.Followers.Count,
                        },
                        isFollowed = true
                    };
                }
            
                return new UserProfileVisited
                {
                    userData = new UserProfileData
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Likes = user.Likes,
                        PostsNumber = user.PostsNumber,
                        FollowersNumber = user.Followers.Count,
                    },
                    isFollowed = false
                };
            }

            throw new UserServiceException(ErrorMessages.UserNotFound, "1");
        }


        public bool AddFollowerIdToUserByUsername(string username, Guid UserId)
        {
            var followingUser = _context.Users.FirstOrDefault(u => u.Username == username);
            var follower = _context.Users.FirstOrDefault(u => u.Id == UserId);

            if(followingUser != null && follower != null) {

                if (follower.Following.Contains(followingUser.Id))
                {
                    followingUser.Followers.Remove(UserId);
                    follower.Following.Remove(followingUser.Id);

                    _context.Users.Update(followingUser);
                    _context.Users.Update(follower);

                    _context.SaveChanges();
                }
                else
                {
                    followingUser.Followers.Add(UserId);
                    follower.Following.Add(followingUser.Id);

                    _context.Users.Update(followingUser);
                    _context.Users.Update(follower);

                    _context.SaveChanges();
                }

                
                return true;
            }

            return false;
        }
       

        public User GetUserDataByToken(string token) { 

            var userId = TokenUtility.RetriveDataFromToken(token);
            var userData = GetUserById(userId);

            return userData;
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
            if (_userValidator.ValidateUser(user) && IsRegisteredDataValid(user)) 
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
                    Followers = new List<Guid>(),
                    Following = new List<Guid>(),
                    Posts = new List<Guid>(),
                });

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

        private bool IsRegisteredDataValid(User user)
        {
            var userWithTheSameData = _context.Users.FirstOrDefault(u => u.Email == user.Email || u.Username == user.Username);

            if (userWithTheSameData == null)
            {
                return true;
            }
            return false;
        }
    }
}

 