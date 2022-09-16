using Microsoft.EntityFrameworkCore;
using StudInfoDataAccess.Data;
using StudInfoDataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoDataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> dbset;

        public Repository(AppDbContext db)
        {
            _db = db;
            dbset = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public void AddRange(IEnumerable<T> entity)
        {
            dbset.AddRange(entity);
        }

        public T GetOne(Expression<Func<T, bool>> filter, string? includeOthers = null)
        {
            IQueryable<T> query = dbset;

            query = query.Where(filter);     //.AsNoTracking();

            if (includeOthers != null)
            {
                if (includeOthers.Contains(","))
                {
                    var arrObj = includeOthers.Split(",", StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < arrObj.Length; i++)
                    {
                        query = query.Include(arrObj[i].Trim());
                    }
                }
                
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeOthers = null)
        {
            IQueryable<T> query = dbset;

            if (filter != null)
                query = query.Where(filter);

            if (includeOthers != null)
            {
                foreach (var item in includeOthers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item.Trim());
                }
            }

            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }


        public void RemoveRange(IEnumerable<T> entity)
        {
            dbset.RemoveRange(entity);
        }

        public void UpdateRange(IEnumerable<T> entity)
        {
            dbset.UpdateRange(entity);
        }
    }

}
