using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using Paiement_1830974.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paiement_1830974.Data.Context
{
    public class CiusssContext : DbContext
    {
        public DbSet<Logs> Logs { get; set; }
        public DbSet<RevenueByType> RevenueByTypes { get; set; }
        public DbSet<OtherValues> OtherValues { get; set; }
        public DbSet<Reciept> Reciepts { get; set; }

        public CiusssContext(DbContextOptions<CiusssContext> options) : base(options) { }
    }
}
