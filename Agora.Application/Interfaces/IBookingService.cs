using Agora.Application.DTOs;
using Agora.Domain.Abstractions;

namespace Agora.Application.Interfaces
{
    public interface IBookingService
    {
        Task<Result<bool>> CreateAsync(CreateBookingDto dto, CancellationToken cancellationToken = default);
    }
}
