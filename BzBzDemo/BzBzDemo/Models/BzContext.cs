using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BzBzDemo.Models
{
    public class BzContext : DbContext
    {
        public BzContext()
        {
        }

        public BzContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Factory> Factories { get; set; }

        public DbSet<CountryPerson> CountriesPersons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { 
            optionsBuilder.UseSqlServer(@"Server=.;Database=BzDatabase;Integrated Security=True;Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CountryPerson>(entity =>
            {
                entity.HasKey(cp => new { cp.CountryId, cp.PersonId });
            });

            base.OnModelCreating(modelBuilder);
        }


    }
}
