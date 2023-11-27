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
       
        public TimeSpan ParseStartTime()
        {

            string[] values = this.BookingTime.Split(":");

            var startTime = new TimeSpan(int.Parse(values[0]), int.Parse(values[1]), 0);

            return startTime;
        }


        public TimeSpan ParseEndTime()
        {

            string[] values = this.BookingTime.Split(":");

            var endTime = new TimeSpan(int.Parse(values[0]), int.Parse(values[1]), 0);

            return endTime.Add(new TimeSpan(0, 59, 0));
        }

    }
}
