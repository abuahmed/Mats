using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MATS.Repository;
using MATS.Repository.Interfaces;
using MATS.DAL;
using MATS.DAL.Interfaces;
using MATS.WPF.ViewModel;
using GalaSoft.MvvmLight;
using Microsoft.Practices.Unity;
using MATS.Core;
using MATS.Core.Enumerations;
using System.IO;
using MATS.OA;

namespace MATS.WPF
{
    /// <summary>
    /// Here the DI magic come on.
    /// </summary>
    public class Bootstrapper
    {
        public IUnityContainer Container { get; set; }

        public Bootstrapper()
        {
            Container = new UnityContainer();
            ConfigureContainer();
        }

        private void ConfigureContainer()
        {
            //Singleton.Edition = MATSEdition.ServerEdition;//Compact For Clients and Server for main Office
            /**Uncomment below for Server Edition*/
            Singleton.Edition = MATSEdition.ServerEdition;
            Singleton.SqlceFileName = "MatsDb1";

            /**Uncomment below for Compact Edition*/
            //Singleton.Edition = MATSEdition.CompactEdition;
            //var path = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + "\\Mats\\";
            //if (!Directory.Exists(path))
            //    Directory.CreateDirectory(path);
            //var pathfile = Path.Combine(path, "MatsDb.sdf");
            //Singleton.SqlceFileName = pathfile;

            Container.RegisterType<IDbContext, MATSDbContext>(new ContainerControlledLifetimeManager());
            Container.RegisterInstance<IDbContext>(new DbContextFactory().Create());

            Container.RegisterType<IUnitOfWork, UnitOfWork>();
            Container.RegisterInstance<IUnitOfWork>(new UnitOfWork(Container.Resolve<IDbContext>()));

            const string connectionStringName = @"Data Source=.;Initial Catalog=MatsDb1;User ID=sa;pwd=amihan; Connect Timeout=2000; Pooling='true'; Max Pool Size=200";            
            Container.RegisterType<IMATSServerDbContextUnitOfWork, MATSServerDbContext>();
            Container.RegisterInstance<IMATSServerDbContextUnitOfWork>(new MATSServerDbContext(connectionStringName));

            Container.RegisterType<MainViewModel>();
        }
    }

}
