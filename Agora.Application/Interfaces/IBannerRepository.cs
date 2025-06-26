using Agora.Application.DTOs;
using Agora.Domain.Entities;

namespace Agora.Application.Interfaces
{
    public interface IBannerRepository
    {
        Task<List<Banner>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Banner?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Banner banner, CancellationToken cancellationToken = default);
        void Update(Banner banner);
        void Remove(Banner banner);
    }
}
