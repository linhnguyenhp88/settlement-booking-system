using Microsoft.EntityFrameworkCore;
using SettlementBookingSystem.Domain.Entites;
using SettlementBookingSystem.Domain.Interfaces;
using SettlementBookingSystem.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly SettlementBookingSystemContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public BookingRepository(SettlementBookingSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException(nameof(Booking));

            await _context.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Booking>> GetBookingsAsync(DateTime bookingTime)
        {
            var query = await _context.Bookings.Where(x => x.BookingStartTime == bookingTime).ToListAsync();
            return query;
        }
}
    }
