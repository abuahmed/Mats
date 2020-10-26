using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using MATS.Core.Models;

namespace MATS.DAL.Mappings
{
    public class AddressMap : EntityTypeConfiguration<AddressDTO>
    {
        public AddressMap() {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.City)
                //.IsRequired()
               .HasMaxLength(15);

            this.Property(t => t.SubCity)
                .HasMaxLength(15);

            this.Property(t => t.Kebele)
                //.IsRequired()
                .HasMaxLength(40);

            this.Property(t => t.HouseNumber)
                //.IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Telephone)
                //.IsRequired()
                .HasMaxLength(24);

            this.Property(t => t.Mobile)
                //.IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.StreetAddress)
                //.IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.PoBox)
                .HasMaxLength(60);

            this.Property(t => t.Fax)
                .HasMaxLength(24);

            this.Property(t => t.PrimaryEmail)
                //.IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AlternateEmail)
                .HasMaxLength(15);

            // Table & Column Mappings
            ToTable("Addresses");
            Property(t => t.Id).HasColumnName("Id");
         
        }


    }
}
