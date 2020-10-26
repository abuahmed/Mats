using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using MATS.DAL.Interfaces;
using MATS.Repository.Interfaces;

namespace MATS.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(IDbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("context");

            _context = dbContext;
        }

        internal IDbSet<T> GetDbSet<T>() where T : class
        {
            return _context.Set<T>();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        //Dispose
        private bool _disposed;
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();

            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                            .MakeGenericType(typeof(T)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }
    }
}
