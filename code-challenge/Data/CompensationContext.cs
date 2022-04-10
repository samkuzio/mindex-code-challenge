using challenge.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Data
{
    public class CompensationContext : DbContext
    {
        public DbSet<Compensation> Compensations { get; set; }

        public CompensationContext(DbContextOptions<CompensationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Compensation>()
                .HasKey(c => c.Employee);
        }
    }
}
