using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using MATS.Core.Models;

namespace MATS.DAL.Mappings
{
    public class ClientMap : EntityTypeConfiguration<ClientDTO>
    {
        public ClientMap()
        {
            //Primary Key
            this.HasKey(c => c.Id);

            //Properties
            Property(c => c.Id)
                .IsRequired();

            Property(c => c.ClientCode)
                .IsRequired();

            Property(c => c.DisplayName)
                .IsRequired()
                .HasMaxLength(50);

            //Property(c => c.ClientStatus)
            //    .IsRequired();
           
            // Table & Column Mappings
            ToTable("Clients");

            //Relationships
            HasOptional(t => t.Address)
                   .WithMany()
                   .HasForeignKey(t => t.AddressId);

            //this.HasOptional(t => t.Contact)
            //  .WithMany()
            //  .HasForeignKey(t => new { t.ContactId });
           
                
        }
    }
}
