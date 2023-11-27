using MediatR;
using SettlementBookingSystem.Application.Bookings.Dtos;
using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingCommand : IRequest<BookingDto>
    {
        public string Name { get; set; }
       
        public string BookingTime { get; set; }          
    }
}
