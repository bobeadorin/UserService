using Microsoft.EntityFrameworkCore;
using UserService.Constant;
using UserService.Models;
using UserService.Utility;

namespace UserService.DbConnection
{
    public class AppDbContext : DbContext
    {
        public IConfiguration _config { get; set; }

        public AppDbContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DatabaseConnection"));
        }
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<User>().Property(u => u.Password).HasConversion(p => p.ToString(), p => Hashing.toSHA256(p));

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = new Guid("d3e37998-8cbf-4162-ba83-b7f28758b033"),
                Email = DbSeedData.Email,
                Password = Hashing.toSHA256(DbSeedData.Password),
                Username = DbSeedData.Username,
                FirstName = DbSeedData.FirstName,
                LastName = DbSeedData.LastName,
                Country = DbSeedData.Country,
                Currency = DbSeedData.Currency,
                Address = DbSeedData.Address,
                PhoneNumber = DbSeedData.PhoneNumber,
                Likes = DbSeedData.Likes,
                PostsNumber = DbSeedData.PostsNumber,
            });

            modelBuilder.Entity<ServiceLogin>().HasData(new ServiceLogin
            {   Id = new Guid("b27bcc3a-8ac1-4e59-a9e6-ab1c86bec745"),
                Username = ServiceAuthData.Username,
                Password = Hashing.toSHA256(ServiceAuthData.Password)
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<JwtRefreshToken> jwtRefreshTokens { get; set; }
        public DbSet<ServiceLogin> serviceLogin { get; set; }
    }
}
