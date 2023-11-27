using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SettlementBookingSystem.Application.Bookings.Dtos;
using SettlementBookingSystem.Application.Exceptions;
using SettlementBookingSystem.Domain.Entites;
using SettlementBookingSystem.Domain.Interfaces;
using SettlementBookingSystem.Infrastructure.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingDto>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<CreateBookingCommandHandler> _logger;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository,
            ILogger<CreateBookingCommandHandler> logger)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var bookingTimeStart = new System.TimeSpan(9, 0, 0);
            var bookingTimeEnd = new System.TimeSpan(16, 0, 0);

            if (ParseStartTime(request.BookingTime) < bookingTimeStart)
            {
                throw new ValidationException("BookingTime must be greater than or equal to 09:00:00");
            }

            if (ParseStartTime(request.BookingTime) > bookingTimeEnd)
            {
                throw new ValidationException("BookingTime must be less than or equal to 16:00:00");
            }

            var BookingEntities = await _bookingRepository.GetBookingsAsync(Convert.ToDateTime(request.BookingTime));

            if ((BookingEntities.Any() && BookingEntities != null))
            {
                if (BookingEntities.Count >= 4)
                {
                    throw new ConflictException("All settlements at a booking time are reserved at this time.");
                }           
            }

            var booking = new Booking
            {
                BookingEndTime = Convert.ToDateTime(ParseEndTime(request.BookingTime).ToString()),
                BookingStartTime = Convert.ToDateTime(request.BookingTime),
                Name = request.Name,
                Id = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.Now
            };

            _logger.LogInformation($"Creating Booking with Id :  {booking.Id}");

            await _bookingRepository.AddAsync(booking);
            return new BookingDto { BookingId = booking.Id };
        }

        private TimeSpan ParseStartTime(string bookingTime)
        {
            string[] values = bookingTime.Split(":");
            var startTime = new TimeSpan(int.Parse(values[0]), int.Parse(values[1]), 0);

            return startTime;
        }

        private TimeSpan ParseEndTime(string bookingTime)
        {
            string[] values = bookingTime.Split(":");
            var endTime = new TimeSpan(int.Parse(values[0]), int.Parse(values[1]), 0);

            return endTime.Add(new TimeSpan(0, 59, 0));
        }
    }
}
