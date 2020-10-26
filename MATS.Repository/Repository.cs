using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using MATS.DAL.Interfaces;
using MATS.Core;
using MATS.Repository.Interfaces;
using System.Linq.Expressions;

namespace MATS.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        internal IDbContext Context;
        private IDbSet<TEntity> _dbSet;

        public Repository(IDbContext context)
        {
            Context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbSet;
        }
        public IQueryable<TEntity> GetAllIncludingChilds(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;

            //if (includeProperties != null)
            //includeProperties.ForEach(i => { query = query.Include(i); });

            foreach (var navigationProperty in includeProperties)
            {
                query = query.Include(navigationProperty);
            }

            return query;
        }
        public virtual TEntity GetFirst()
        {
            return _dbSet.FirstOrDefault();
        }
        public virtual TEntity GetLast()
        {
            return _dbSet.LastOrDefault();
        }
        public virtual TEntity FindById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Add(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            var objectState = entity as EntityBase;
            if (objectState != null)
                objectState.ObjectState = ObjectState.Deleted;
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            //if (Context.Entry(entity).State == EntityState.Detached)
            //{
            //    _dbSet.Attach(entity);
            //}
            _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }
        public virtual int Count()
        {
            return _dbSet.Count();
        }

        internal IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includeProperties != null)
                includeProperties.ForEach(i => { query = query.Include(i); });

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            return query;
        }


    }
}
