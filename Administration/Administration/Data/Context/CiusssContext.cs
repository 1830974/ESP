using Administration.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Administration.Data.Context
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

        public CiusssContext(DbContextOptions<CiusssContext> options) : base(options) { }
    }
}
