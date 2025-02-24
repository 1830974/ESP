using ParkingApp_1830974.Models;
using Microsoft.EntityFrameworkCore;

namespace ParkingApp_1830974.Data.Context
{
    public class CiusssContext : DbContext
    {
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }

        public CiusssContext(DbContextOptions<CiusssContext> options) : base(options) { }
    }
}
