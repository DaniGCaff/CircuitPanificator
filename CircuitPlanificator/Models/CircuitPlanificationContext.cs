using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.Entity;
using System.Data.Common;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace CircuitPlanificator.Models
{
    public class CircuitPlanificationContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Airports> Airports { get; set; }
        public DbSet<Routes> Routes { get; set; }


        public CircuitPlanificationContext()
              : base()
        {

        }
        // Constructor to use on a DbConnection that is already opened
        public CircuitPlanificationContext(DbConnection existingConnection, bool contextOwnsConnection)
          : base(existingConnection, contextOwnsConnection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<City>().MapToStoredProcedures();
        }
    }

    public class City
    {
        [Key]
        public int idCity { get; set; }

        public string name { get; set; }

        public string code { get; set; }

        public string country_code { get; set; }
    }

    public class Airports
    {
        [Key]
        public string iataCode { get; set; }

        public string name { get; set; }

        public float latitude { get; set; }

        public float longitude { get; set; }

        public string regionCode { get; set; }

        public int idCity { get; set; }
    }

    public class Routes
    {
        [Key]
        public int idRoute { get; set; }

        public string iataCodeDepart { get; set; }
        
        public string iataCodeArrive { get; set; }
    }


}

