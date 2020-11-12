﻿using Entities;
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
        public CreateDbContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<CreateDbContext>(new DropCreateDatabaseAlways<CreateDbContext>());
        }
        public DbSet<DenemeEntity> DenemeEntities { get; set; }
    }
}
