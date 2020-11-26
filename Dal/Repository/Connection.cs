using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repository
{
    public static class Connection
    {
        public static string DatabaseConnection(string connectionString)
        {
            var server = new Microsoft.SqlServer.Management.Smo.Server(@"DESKTOP-8QRCF5A\SQLEXPRESS");
            List<string> alldatabases = new List<string>();

            foreach (Microsoft.SqlServer.Management.Smo.Database db in server.Databases)
            {
                alldatabases.Add(db.Name);
            }
            string databaseName = alldatabases.Find(d => d == connectionString);
            return databaseName;
        }
    }
}


