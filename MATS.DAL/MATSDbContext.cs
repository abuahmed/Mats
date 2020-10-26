using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using MATS.Core;
using MATS.Core.Enumerations;
using MATS.Core.Models;
using MATS.DAL.Mappings;

namespace MATS.DAL
{
    public class MATSDbContext : DbContextBase
    {
        //public MATSDbContext(string conString)
        //    : base(conString)
        //{
        //    Database.SetInitializer<MATSDbContext>(new MigrateDatabaseToLatestVersion<MATSDbContext, Configuration>());
        //    Configuration.ProxyCreationEnabled = false;
        //}

        public MATSDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MATSDbContext, Configuration>());
            Configuration.ProxyCreationEnabled = false;
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new ListMap());
            modelBuilder.Configurations.Add(new ProductActivationMap());

            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new ContactMap());
            modelBuilder.Configurations.Add(new ClientMap());
            modelBuilder.Configurations.Add(new AttachmentMap());
            modelBuilder.Configurations.Add(new RemarkMap());

            modelBuilder.Configurations.Add(new TicketMap());

            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new RoleMap());

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

    public class DbContextFactory : IDbContextFactory<MATSDbContext>
    {
        public MATSDbContext Create()
        {
            if (Singleton.Edition == MATSEdition.CompactEdition)
            {
                var sqlCeConString = "Data Source=" + Singleton.SqlceFileName + ";Max Database Size=4091;Password=mat3P@ssw0rd";// "Data Source=E:\\DB_Test\\oneface\\MATSDb.sdf;Password=1fac3P@ssw0rd";
                var sqlce = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");//, path, pathfile);
                return new MATSDbContext(sqlce.CreateConnection(sqlCeConString), true);
            }
            //data source should be contant value="MATSSERVER" "matsserver"
            string sQlServConString = "data source=.;initial catalog=" + Singleton.SqlceFileName + ";user id=sa;password=amihan";
            var sql = new SqlConnectionFactory(sQlServConString);
            return new MATSDbContext(sql.CreateConnection(sQlServConString), true);
        }
    }

    public class Configuration : DbMigrationsConfiguration<MATSDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MATSDbContext context)
        {
            var cust = context.Set<ClientDTO>().Find(1);
            if (cust == null)
            {
                #region Seed Users
                if (Singleton.Edition == MATSEdition.ServerEdition)
                {
                    #region Myco User
                    context.Set<UserDTO>().Add(new UserDTO
                    {
                        UserId = 1,
                        UserName = "mycoadmin",
                        Password = CommonUtility.Encrypt("P@55w0rd123!"),
                        Status = UserTypes.Active,
                        FullName = "Super Admin"
                    });
                    context.Set<UserDTO>().Add(new UserDTO
                    {
                        UserId = 2,
                        UserName = "mycouser",
                        Password = CommonUtility.Encrypt("P@55w0rd!"),
                        Status = UserTypes.Waiting,
                        FullName = "Myco User"
                    });
                    #endregion

                    #region Seed Default Client
                    context.Set<ClientDTO>().Add(new ClientDTO
                    {
                        Id = 1,
                        ClientCode = "CL0001",
                        ProductKey = "12345-67890-12345-67890",
                        DisplayName = "Default Client",
                        ContactName = "Abebe Mamo",
                        NoOfActivations = 0,
                        NoOfAllowedPcs = 1,
                        ExpiryDuration = 365,
                        ContactTitle = "General Manager",
                        ClientStatus = ClientStatus.Active,
                        Address = new AddressDTO
                        {
                            Country = "Ethiopia",
                            City = "Addis Abeba",
                            Mobile = "0911111111",
                            PrimaryEmail = "default@yahoo.com"
                        }
                    });
                    #endregion
                }
                else
                {
                    #region Client User
                    context.Set<UserDTO>().Add(new UserDTO
                    {
                        UserId = 1,
                        UserName = "matsadmin",
                        Password = CommonUtility.Encrypt("P@ssw0rd"),
                        Status = UserTypes.Waiting,
                        FullName = "System Admin"
                    });
                    context.Set<UserDTO>().Add(new UserDTO
                    {
                        UserId = 2,
                        UserName = "matsuser",
                        Password = CommonUtility.Encrypt("pa12345"),
                        Status = UserTypes.Waiting,
                        FullName = "System User"
                    });
                    #endregion
                }
                #endregion

             

                #region City Lists
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.City, DisplayName = "ABHA" });
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.City, DisplayName = "GASSIM" });
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.City, DisplayName = "QASSIM" });
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.City, DisplayName = "RIYADH" });
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.City, DisplayName = "JEDDAH" });
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.City, DisplayName = "DAMMAM" });
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.City, DisplayName = "JIZAN" });
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.City, DisplayName = "TAYIF" });
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.City, DisplayName = "NAJRAN" });
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.City, DisplayName = "TABOUK" });

                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.City, DisplayName = "DUBAI" });
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.City, DisplayName = "KUWAIT" });
                #endregion

                #region AirLines Lists
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.AirLine, DisplayName = "Ethiopian" });
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.AirLine, DisplayName = "Saudi" });
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.AirLine, DisplayName = "Emirates" });
                context.Set<ListDTO>().Add(new ListDTO { Type = ListTypes.AirLine, DisplayName = "Yemen" });
                #endregion
            }
            base.Seed(context);
        }
    }

}
