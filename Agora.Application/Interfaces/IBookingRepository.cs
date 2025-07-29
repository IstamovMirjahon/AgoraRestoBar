using Agora.Domain.Entities;

namespace Agora.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
        Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
