using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class CreateDbContext : DbContext
    {
        public CreateDbContext()
        {
            Database.SetInitializer<CreateDbContext>(new CreateDatabaseIfNotExists<CreateDbContext>());
        }
        public CreateDbContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<CreateDbContext>(new CreateDatabaseIfNotExists<CreateDbContext>());
        }
        public DbSet<CustomerInfo> CustomerInfos { get; set; }

        public DbSet<Remainder> Remainders { get; set; }

    }
}