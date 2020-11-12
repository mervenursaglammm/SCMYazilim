using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class Repository<T> where T : class
    {
        private DatabaseContext context = new DatabaseContext();
        public T Find(Expression<Func<T, bool>> where)
        {
            return context.Set<T>().Where(where).FirstOrDefault();
        }
        public List<T> List()
        {
            return context.Set<T>().ToList();
        }
        public List<T> List(Expression<Func<T, bool>> where)
        {
            return context.Set<T>().Where(where).ToList();
        }
        public int Insert(T Obj)
        {
            context.Set<T>().Add(Obj);
            return Save();
        }
        public int Delete(T Obj)
        {
            context.Set<T>().Remove(Obj);
            return Save();
        }
        public int Update(T Obj)
        {
            return Save();
        }
        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
