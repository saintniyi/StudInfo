using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoDataAccess.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        void AddRange(IEnumerable<T> entity);

        T GetOne(Expression<Func<T,bool>> filter, string? includeOthers = null);

        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeOthers = null);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity);

        void UpdateRange(IEnumerable<T> entity);


    }
}
