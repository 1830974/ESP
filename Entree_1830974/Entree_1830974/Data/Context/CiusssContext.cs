using Entree_1830974.Models;
using Microsoft.EntityFrameworkCore;

namespace Entree_1830974.Data.Context
{
    public class CiusssContext : DbContext
    {
        public DbSet<Logs> Logs { get; set; }
        public DbSet<Parking> Parking { get; set; }
        public DbSet<Ticket> Tickets { get; set; }


        public CiusssContext(DbContextOptions<CiusssContext> options) : base(options) { }
    }
}
