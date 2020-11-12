using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=DenemeConnection")
        {

        }
        public DbSet<Customer> Customers { get; set; }
    }
}
