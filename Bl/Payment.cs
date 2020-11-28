using Dal;
using Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl
{
    public class Payment<T> where T : class
    {
        private static CreateDbContext createContext;
        string baseConnectionString = ConfigurationManager.ConnectionStrings["BaseConnectionString"].ConnectionString;

        public float GetRemainder(string databaseName)
        {
            createContext = new CreateDbContext(string.Format(baseConnectionString, databaseName));
            float remainder = createContext.Remainders.Max(x => x.remainder);
            return remainder;
        }
    }
}
