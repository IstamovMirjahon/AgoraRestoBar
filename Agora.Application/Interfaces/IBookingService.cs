using Agora.Application.DTOs;
using Agora.Domain.Abstractions;

namespace Agora.Application.Interfaces
{
    public interface IBookingService
    {
        Task<Result<bool>> CreateAsync(CreateBookingDto dto, CancellationToken cancellationToken = default);
        Task<Result<List<BookingDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<bool>> ToggleConfirmationAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<List<BookingDto>>> GetUnconfirmedAllAsync(CancellationToken cancellationToken = default);
    }
}
