using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using MATS.Core.Models;

namespace MATS.DAL.Mappings
{
    public class TicketMap : EntityTypeConfiguration<TicketDTO>
    {
        public TicketMap() 
        {            
            // Primary Key
            HasKey(t => t.Id);

            //Properties
            Property(t => t.PassengerPassportNumber)
               .IsRequired()
               .HasMaxLength(15);

            Property(t => t.PassengerFullName)
               .IsRequired()
               .HasMaxLength(50);

            Property(t => t.City)
              .IsRequired()
              .HasMaxLength(20);

            Property(t => t.Route)              
              .HasMaxLength(150);

            Property(t => t.RequestedDate)
                .IsRequired();
            

            //Property(t => t.TicketStatus)
            //  .IsRequired();

            //Property(t => t.TypeOfTrip)
            //  .IsRequired();

            //Property(t => t.TicketNumber)
            //  .HasMaxLength(25);

            // Table & Column Mappings
            ToTable("Tickets");
            Property(t => t.Id).HasColumnName("Id");

            //Relationships
            //this.HasOptional(t => t.Client)
            //    .WithMany(t=>t.Tickets)
            //    .Map(m => m.MapKey("Id"));
            //this.HasRequired(t => t.Client)
            //  .WithMany(t => t.Tickets)
            //  .HasForeignKey(t => t.ClientId);



        }
    }
}
