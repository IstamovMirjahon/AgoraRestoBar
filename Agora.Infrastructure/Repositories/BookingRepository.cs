using Agora.Application.Interfaces;
using Agora.Domain.Entities;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            await _context.Bookings.AddAsync(booking, cancellationToken);
        }

        public Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _context.Bookings
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
