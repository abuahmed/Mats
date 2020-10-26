using System.Data.Entity.ModelConfiguration;
using MATS.Core.Models;

namespace MATS.DAL.Mappings
{
    public class RoleMap : EntityTypeConfiguration<RoleDTO>
    {
        public RoleMap()
        {
            // Primary Key
            HasKey(t => t.RoleId);

            // Properties
            Property(t => t.RoleDescription)
               .IsRequired();

            // Table & Column Mappings
            ToTable("Roles");

            //HasMany(u => u.Users)
            //    .WithMany(u => u.Roles)
            //    .Map(a => a.ToTable("UserRoles")
            //                .MapLeftKey("RoleId")
            //                .MapRightKey("UserId"));
                   
        }
    }
    public class UsersInRolesMap : EntityTypeConfiguration<UsersInRoles>
    {
        public UsersInRolesMap()
        {
            // Primary Key
            //this.HasKey(t => {t.RoleId,t.UserId});

            // Properties
            //Property(t => t.RoleDescription)
              // .IsRequired();

            // Table & Column Mappings
            ToTable("UsersInRoles");

            //Relationships
            HasRequired(t => t.User)
             .WithMany(e => e.Roles)
             .HasForeignKey(t => t.UserId);

            HasRequired(t => t.Role)
             .WithMany(e => e.Users)
             .HasForeignKey(t => t.RoleId);

            //HasMany(u => u.Users)
            //    .WithMany(u => u.Roles)
            //    .Map(a => a.ToTable("UserRoles")
            //                .MapLeftKey("RoleId")
            //                .MapRightKey("UserId"));

        }
    }
}
