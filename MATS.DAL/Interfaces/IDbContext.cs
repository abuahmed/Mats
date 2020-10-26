using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace MATS.DAL.Interfaces
{
    public interface IDbContext : IDisposable
    {
        IDbSet<T> Set<T>() where T : class;
        int SaveChanges();
        DbEntityEntry Entry(object o);
        //void Dispose();

        //DbSet<T> Set<T>() where T : class;
    }
}
