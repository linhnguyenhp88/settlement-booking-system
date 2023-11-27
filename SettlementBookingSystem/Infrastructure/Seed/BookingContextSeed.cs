using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using SettlementBookingSystem.Infrastructure.EntityFrameworkCore;
using SettlementBookingSystem.Domain.Entites;

namespace SettlementBookingSystem.Infrastructure.Seed
{
    public class BookingContextSeed
    {
        public void Seed(SettlementBookingSystemContext context)
        {          
            var prodList = new List<Booking>
            {
                new Booking
                { 
                    Id = Guid.NewGuid() ,Name = "Linh", BookingStartTime = DateTime.Now, BookingEndTime = DateTime.Now.AddHours(1), CreatedAt=DateTimeOffset.Now 
                },
                new Booking
                {
                    Id = Guid.NewGuid() ,Name="Keith", BookingStartTime = DateTime.Now, BookingEndTime = DateTime.Now.AddHours(1),CreatedAt=DateTimeOffset.Now 
                },
                new Booking 
                {
                    Id = Guid.NewGuid() ,Name="John", BookingStartTime = DateTime.Now, BookingEndTime = DateTime.Now.AddHours(1), CreatedAt=DateTimeOffset.Now 
                }
            };

            context.Bookings.AddRange(prodList);
            context.SaveChanges();
        }
    }
}
