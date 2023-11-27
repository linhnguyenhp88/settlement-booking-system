using SettlementBookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Domain.Entites
{
    public class Booking : Entity, IAggregateRoot
    {
        public Booking() { }

        public DateTimeOffset CreatedAt { get; set; }
        public string Name { get; set; }
        public DateTime BookingStartTime { get; set; }
        public DateTime BookingEndTime { get; set; }
    }
}
