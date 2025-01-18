using HomeExam.AlertManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeExam.AlertManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<UserFlight> UsersFlights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
