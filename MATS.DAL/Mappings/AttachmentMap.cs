using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using MATS.Core.Models;

namespace MATS.DAL.Mappings
{
    public class AttachmentMap : EntityTypeConfiguration<AttachmentDTO>
    {
        public AttachmentMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            Property(t => t.AttachmentName)
               .IsRequired()
               .HasMaxLength(75);
         

            // Table & Column Mappings
            ToTable("Attachments");
            Property(t => t.Id).HasColumnName("Id");

            //Relationships
            this.HasRequired(t => t.Ticket)
                .WithMany(t => t.Attachments)
                .HasForeignKey(t=>t.TicketId)
                .WillCascadeOnDelete(true);
                //.Map(m => m.MapKey("Id"));

        }


    }
}

