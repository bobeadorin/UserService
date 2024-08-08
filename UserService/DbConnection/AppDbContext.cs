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
                Id = Guid.NewGuid(),
                Email = DbSeedData.Email,
                Password = Hashing.toSHA256(DbSeedData.Password),
                Username = DbSeedData.Username,
                FirstName = DbSeedData.FirstName,
                LastName = DbSeedData.LastName,
                Country = DbSeedData.Country,
                Currency = DbSeedData.Currency,
                Address = DbSeedData.Address,
                PhoneNumber = DbSeedData.PhoneNumber
            });

        }


        public DbSet<User> Users { get; set; }
        public DbSet<JwtRefreshToken> jwtRefreshTokens { get; set; }
    }
}
