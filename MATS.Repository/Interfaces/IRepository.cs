using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MATS.Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        IQueryable<TEntity> GetAllIncludingChilds(params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetFirst();
        TEntity GetLast();
        TEntity FindById(object id);        
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        int Count();
    }
}
