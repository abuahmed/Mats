using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using MATS.Core.Models;

namespace MATS.DAL.Mappings
{
    public class RemarkMap : EntityTypeConfiguration<RemarkDTO>
    {
        public RemarkMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            Property(t => t.RemarkText)
               .IsRequired();
          

            // Table & Column Mappings
            ToTable("Remarks");
            Property(t => t.Id).HasColumnName("Id");

            //Relationships
            //this.HasRequired(t => t.Ticket)
            //    .WithMany(t=>t.Remarks)
            //    .Map(m => m.MapKey("Id"));
            this.HasRequired(t => t.Ticket)
              .WithMany(t => t.Remarks)
              .HasForeignKey(t => t.TicketId);
        }


    }
}

