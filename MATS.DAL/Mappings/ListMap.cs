using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using MATS.Core.Models;

namespace MATS.DAL.Mappings
{
    public class ListMap : EntityTypeConfiguration<ListDTO>
    {
        public ListMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            Property(t => t.DisplayName)
               .IsRequired()
               .HasMaxLength(75);
                        
            // Table & Column Mappings
            ToTable("Lists");
            Property(t => t.Id).HasColumnName("Id");

            //Relationships
           
        }


    }
}

