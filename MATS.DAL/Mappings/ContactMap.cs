using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using MATS.Core.Models;

namespace MATS.DAL.Mappings
{
    public class ContactMap : EntityTypeConfiguration<ContactDTO>
    {
        public ContactMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            Property(t => t.ContactName)
               .IsRequired()
               .HasMaxLength(45);

            Property(t => t.ContactTitle)
                .HasMaxLength(15);

            // Table & Column Mappings
            ToTable("Contacts");
            Property(t => t.Id).HasColumnName("Id");

            //Relationships
            this.HasRequired(t => t.Address)
              .WithMany()
              .HasForeignKey(t => t.AddressId);
        }


    }
}

