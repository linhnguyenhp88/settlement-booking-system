using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Exceptions;
using SettlementBookingSystem.Domain.Entites;
using SettlementBookingSystem.Domain.Interfaces;
using SettlementBookingSystem.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SettlementBookingSystem.Application.UnitTests
{
    public class CreateBookingCommandHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
         
        public CreateBookingCommandHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();        
        }

        [Fact]
        public async Task GivenValidBookingTime_WhenNoConflictingBookings_ThenBookingIsAccepted()
        {
            var command = new CreateBookingCommand
            {
                Name = "Test",
                BookingTime = "09:15",
            };
          
            _bookingRepositoryMock.Setup(bookingRepo => bookingRepo.GetBookingsAsync(It.IsAny<DateTime>()))
           .Returns(Task.FromResult(FakeBookings()));

            var LoggerMock = new Mock<ILogger<CreateBookingCommandHandler>>();
            var handler = new CreateBookingCommandHandler(_bookingRepositoryMock.Object, LoggerMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.BookingId.Should().NotBeEmpty();
        }

        [Fact]
        public void GivenOutOfHoursBookingTime_WhenBooking_ThenValidationFails()
        {
            var command = new CreateBookingCommand
            {
                Name = "Test",
                BookingTime = "00:00",
            };

            _bookingRepositoryMock.Setup(bookingRepo => bookingRepo.GetBookingsAsync(It.IsAny<DateTime>()))
            .Returns(Task.FromResult(FakeBookings()));

            var LoggerMock = new Mock<ILogger<CreateBookingCommandHandler>>();
            var handler = new CreateBookingCommandHandler(_bookingRepositoryMock.Object, LoggerMock.Object);

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void GivenValidBookingTime_WhenBookingIsFull_ThenConflictThrown()
        {
            var command = new CreateBookingCommand
            {
                Name = "Test",
                BookingTime = "09:15",
            };

            _bookingRepositoryMock.Setup(bookingRepo => bookingRepo.GetBookingsAsync(It.IsAny<DateTime>()))
           .Returns(Task.FromResult(FakeBookingsV1()));

            var LoggerMock = new Mock<ILogger<CreateBookingCommandHandler>>();
            var handler = new CreateBookingCommandHandler(_bookingRepositoryMock.Object, LoggerMock.Object);

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            act.Should().Throw<ConflictException>();
        }

        private List<Booking> FakeBookingsV1()
        {
            DateTime bookingTime = DateTime.ParseExact("2023-11-17 09:15:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            return new List<Booking>
            {
                new Booking {Id = Guid.NewGuid() ,Name = "Linh", BookingStartTime = bookingTime, BookingEndTime = bookingTime.AddHours(1), CreatedAt=DateTimeOffset.Now},
                new Booking {Id = Guid.NewGuid() ,Name = "Kevin", BookingStartTime = bookingTime, BookingEndTime =  bookingTime.AddHours(1), CreatedAt=DateTimeOffset.Now},
                new Booking {Id = Guid.NewGuid() ,Name = "Peter", BookingStartTime = bookingTime, BookingEndTime =  bookingTime.AddHours(1), CreatedAt=DateTimeOffset.Now},
                new Booking {Id = Guid.NewGuid() ,Name = "Daniel", BookingStartTime = bookingTime, BookingEndTime =  bookingTime.AddHours(1), CreatedAt=DateTimeOffset.Now}
            };
        }

        private List<Booking> FakeBookings()
        {
            return new List<Booking>
            {
                new Booking {Id = Guid.NewGuid() ,Name = "Linh", BookingStartTime = DateTime.Now, BookingEndTime = DateTime.Now.AddHours(1), CreatedAt=DateTimeOffset.Now}
            };
        }
    }
}
