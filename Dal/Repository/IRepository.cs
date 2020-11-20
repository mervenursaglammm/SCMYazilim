using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repository
{
    interface IRepository<T> where T : class
    {
        int Insert(T Obj);
        int Delete(T Obj);
        int Update(T Obj);
        int Save();
        T Find(Expression<Func<T, bool>> where);
        List<T> List();
        List<T> List(Expression<Func<T,bool>> where);

        
    }
}
