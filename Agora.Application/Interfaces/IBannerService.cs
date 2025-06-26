using Agora.Application.DTOs;

namespace Agora.Application.Interfaces
{
    public interface IBannerService
    {
        Task<IEnumerable<BannerDto>> GetAllBannersAsync(CancellationToken cancellationToken = default);
        Task<BannerDto?> GetBannerByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<BannerDto> CreateBannerAsync(CreateAndUpdateBannerDto bannerDto, CancellationToken cancellationToken = default);
        Task<BannerDto> UpdateBannerAsync(Guid id, CreateAndUpdateBannerDto bannerDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteBannerAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ToggleBannerStatusAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
