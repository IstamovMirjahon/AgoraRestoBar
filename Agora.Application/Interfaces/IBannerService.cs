using Agora.Application.DTOs;
using Agora.Domain.Abstractions;

namespace Agora.Application.Interfaces
{
    public interface IBannerService
    {
        Task<Result<List<BannerDto>>> GetAllBannersAsync(CancellationToken cancellationToken = default);
        Task<Result<BannerDto>> GetBannerByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<BannerDto>> CreateBannerAsync(CreateAndUpdateBannerDto bannerDto, CancellationToken cancellationToken = default);
        Task<Result<BannerDto>> UpdateBannerAsync(Guid id, CreateAndUpdateBannerDto bannerDto, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteBannerAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<bool>> ToggleBannerStatusAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
