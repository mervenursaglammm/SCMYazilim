using Dal;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl
{
    public class Payment<T> where T : class
    {
        private BL_Result<CustomerInfo> result1 = new BL_Result<CustomerInfo>();
        private static CreateDbContext createContext;

        /*public BL_Result<Remainder> GetRemainder()
        {
            return createContext.Database.
        }*/
    }
}
