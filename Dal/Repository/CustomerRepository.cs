using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repository
{
    public class CustomerRepository<T> : IRepository<T> where T : class
    {
        CreateDbContext createDbContext = new CreateDbContext("BaseConnectionString");
        public int Delete(T Obj)
        {
            createDbContext.Set<T>().Remove(Obj);
            return Save();
        }

       
        public T Find(Expression<Func<T, bool>> where)
        {
            return createDbContext.Set<T>().Where(where).FirstOrDefault();
        }

        public T Find(Expression<Func<T, bool>> where,string conStr)
        {
            CreateDbContext createDbContext1 = new CreateDbContext(conStr);
            return createDbContext1.Set<T>().Where(where).FirstOrDefault();
        }
        public int Insert(T Obj)
        {
            createDbContext.Set<T>().Add(Obj);
            return Save();
        }

        public List<T> List()
        {
            return createDbContext.Set<T>().ToList();
        }

        public List<T> List(Expression<Func<T, bool>> where)
        {
            return createDbContext.Set<T>().Where(where).ToList();
        }

        public int Save()
        {
            return createDbContext.SaveChanges();
        }

        public int Update(T Obj)
        {
            return Save();
        }
    }
}
