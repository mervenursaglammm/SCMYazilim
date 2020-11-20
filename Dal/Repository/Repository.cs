using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        DatabaseContext databaseContext = new DatabaseContext();
        public int Delete(T Obj)
        {
            databaseContext.Set<T>().Remove(Obj);
            return Save();
        }

      

        public T Find(Expression<Func<T, bool>> where)
        {
            return databaseContext.Set<T>().Where(where).FirstOrDefault();
        }

        public int Insert(T Obj)
        {
          
            databaseContext.Set<T>().Add(Obj);
            return Save();
        }

        public List<T> List()
        {
            return databaseContext.Set<T>().ToList();
        }

        public List<T> List(Expression<Func<T, bool>> where)
        {
            return databaseContext.Set<T>().Where(where).ToList();
        }

        public int Save()
        {
            
                return databaseContext.SaveChanges();
            
            
        }

        public int Update(T Obj)
        {
            return Save();
        }
    }
}
