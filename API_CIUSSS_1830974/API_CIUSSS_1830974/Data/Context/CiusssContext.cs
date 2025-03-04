using API_CIUSSS_1830974.Models;
using Microsoft.EntityFrameworkCore;

namespace API_CIUSSS_1830974.Data.Context
{
    public class CiusssContext : DbContext
    {
        public DbSet<Logs> Logs { get; set; }
        public DbSet<Parking> Parking { get; set; }
        public DbSet<Prices> Prices { get; set; }
        public DbSet<Reciept> Reciepts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OtherValues> OtherValues { get; set; }
        public DbSet<RevenueByType> RevenueByTypes { get; set; }

        public CiusssContext(DbContextOptions<CiusssContext> options) : base(options) { }
    }
}
