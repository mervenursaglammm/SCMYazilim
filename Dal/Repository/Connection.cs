using Microsoft.SqlServer.Management.Common;
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
            //Server = 2.56.154.97; Database = myDataBase; User Id = sa; Password = @acm ^ G6U
            string ServerName ="2.56.154.97";
            string UserName = "sa";
            string Password = "@acm^G6U";
            ServerConnection cn = new ServerConnection(ServerName, UserName, Password);
            var server = new Microsoft.SqlServer.Management.Smo.Server(cn);
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


