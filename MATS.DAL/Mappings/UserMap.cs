using System.Data.Entity.ModelConfiguration;
using MATS.Core.Models;

namespace MATS.DAL.Mappings
{
    public class UserMap : EntityTypeConfiguration<UserDTO>
    {
        public UserMap()
        {
            // Primary Key
            HasKey(t => t.UserId);

            // Properties
            Property(t => t.UserName)
               .IsRequired()
               .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Users");

            //HasMany(u => u.Roles)
            //    .WithMany()
            //    .Map(a => a.ToTable("UserRoles")
            //                .MapLeftKey("UserId")
            //                .MapRightKey("RoleId"));
        }
    }
}
