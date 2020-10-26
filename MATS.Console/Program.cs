using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MATS.Core.Enumerations;
using MATS.Core.Models;
using MATS.DAL;
using MATS.Repository;
using MATS.Repository.Interfaces;

namespace MATS.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MATSDbContext db = new DbContextFactory().Create() )
            using (IUnitOfWork unitOfWork = new UnitOfWork(db))
            {
                var client = new ClientDTO()
                {
                    Id = 1,
                    ClientCode = "CL0001",
                    DisplayName = "Common Client2"
                };
                unitOfWork.Repository<ClientDTO>().Insert(client);

                var ticket = new TicketDTO()
                {
                    City = "Dubai",
                    PassengerFullName = "Ayelech Werkato Demboba",
                    PassengerPassportNumber = "EP1234567",
                    RequestedDate = DateTime.Now,
                    Route = "",
                    TicketNumber = "",
                    LocalStatus = TicketStatus.Book,
                    TypeOfTrip = TypeOfTrips.OneWay,
                    ClientId = client.Id
                };
                unitOfWork.Repository<TicketDTO>().Insert(ticket);

                unitOfWork.Commit();
            }
        }
    }
}
