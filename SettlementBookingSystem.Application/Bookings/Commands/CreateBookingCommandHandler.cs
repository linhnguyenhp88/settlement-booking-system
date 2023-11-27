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

            if (request.ParseStartTime() < bookingTimeStart)
            {
                throw new ValidationException("must be greater than or equal to 09:00:00");
            }

            if (request.ParseStartTime() > bookingTimeEnd)
            {
                throw new ValidationException("must be less than or equal to 16:00:00");
            }

            var BookingEntities = await _bookingRepository.GetBookingsAsync(Convert.ToDateTime(request.ParseStartTime().ToString()));

            if ((BookingEntities.Any() && BookingEntities != null))
            {
                if (BookingEntities.Count >= 4)
                {
                    throw new ConflictException("All settlements at a booking time are reserved at this time.");
                }           
            }

            var booking = new Booking
            {
                BookingEndTime = Convert.ToDateTime(request.ParseEndTime().ToString()),
                BookingStartTime = Convert.ToDateTime(request.ParseStartTime().ToString()),
                Name = request.Name,
                Id = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.Now
            };

            _logger.LogInformation($"Creating Booking with Id :  {booking.Id}");

            await _bookingRepository.AddAsync(booking);
            return new BookingDto { BookingId = booking.Id };
        }


    }
}
