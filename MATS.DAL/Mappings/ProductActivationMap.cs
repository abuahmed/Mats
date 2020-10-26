using System.Data.Entity.ModelConfiguration;
using MATS.Core.Models;

namespace MATS.DAL.Mappings
{
    public class ProductActivationMap : EntityTypeConfiguration<ProductActivationDTO>
    {
        public ProductActivationMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            Property(t => t.ProductKey)
               .IsRequired();

            Property(t => t.RegisteredBIOS_SN)
               .IsRequired();

            // Table & Column Mappings
            ToTable("ProductActivation");

            //Relationships
           
        }
    }
}
