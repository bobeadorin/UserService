namespace UserService.Models
{
    public class UserProfileData
    {
        public Guid Id { get; set; }
        public string Username { get; set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Likes { get; set; }
        public int PostsNumber {  get; set; }
        public int FollowersNumber { get; set; }
    }
}
