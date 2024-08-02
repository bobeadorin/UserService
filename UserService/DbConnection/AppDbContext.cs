using Microsoft.EntityFrameworkCore;
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
            modelBuilder.Entity<User>().Property(u => u.Password).HasConversion(p => p.ToString() , p => Hashing.toSHA256(p));
        }


        public DbSet<User> Users { get; set; }

    }
}
