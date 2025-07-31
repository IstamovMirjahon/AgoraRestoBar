using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Agora.Domain.Abstractions;
using Agora.Domain.Entities;

namespace Agora.Application.Services;

public class BannerService : IBannerService
{
    private readonly IBannerRepository _bannerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public BannerService(
        IBannerRepository bannerRepository,
        IUnitOfWork unitOfWork,
        IFileService fileService)
    {
        _bannerRepository = bannerRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<Result<List<BannerDto>>> GetAllBannersAsync(CancellationToken cancellationToken = default)
    {
        var banners = await _bannerRepository.GetAllAsync(cancellationToken);
        var result = banners.Select(MapToDto).ToList();
        return Result<List<BannerDto>>.Success(result);
    }

    public async Task<Result<BannerDto>> CreateBannerAsync(CreateAndUpdateBannerDto dto, CancellationToken cancellationToken = default)
    {
        string? filePath = null;

        if (dto.MediaUrl != null)
        {
            var extension = Path.GetExtension(dto.MediaUrl.FileName).ToLowerInvariant();

            if (extension is ".png" or ".jpg" or ".jpeg")
                filePath = await _fileService.SaveFileAsync(dto.MediaUrl, "images/banners", cancellationToken);
            else if (extension is ".mp4")
                filePath = await _fileService.SaveFileAsync(dto.MediaUrl, "videos/banners", cancellationToken);
            else
                return Result<BannerDto>.Failure(new Error("Banner.InvalidFormat", "Faqat .png, .jpg, .jpeg, .mp4 fayllarga ruxsat beriladi"));
        }

        var banner = new Banner
        {
            Title = dto.Title,
            Description = dto.Description,
            MediaUrl = filePath!,
            IsActive = dto.IsActive,
            CreateDate = DateTime.UtcNow,
            UpdateDate = DateTime.UtcNow
        };

        await _bannerRepository.AddAsync(banner, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<BannerDto>.Success(MapToDto(banner));
    }

    public async Task<Result<BannerDto>> UpdateBannerAsync(Guid id, CreateAndUpdateBannerDto dto, CancellationToken cancellationToken = default)
    {
        var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
        if (banner is null)
            return Result<BannerDto>.Failure(new Error("Banner.NotFound", "Banner topilmadi"));

        if (dto.MediaUrl != null)
        {
            var extension = Path.GetExtension(dto.MediaUrl.FileName).ToLowerInvariant();

            if (extension is ".png" or ".jpg" or ".jpeg" or ".mp4")
            {
                // Eski faylni o‘chirish
                if (!string.IsNullOrEmpty(banner.MediaUrl))
                {
                    var oldPath = Path.Combine("wwwroot", banner.MediaUrl.TrimStart('/'));
                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }

                string folder = extension == ".mp4" ? "videos/banners" : "images/banners";
                var savedPath = await _fileService.SaveFileAsync(dto.MediaUrl, folder, cancellationToken);
                banner.MediaUrl = savedPath; // Faqat yangi fayl bo‘lsa o‘zgartiramiz
            }
            else
            {
                return Result<BannerDto>.Failure(new Error("Banner.InvalidFormat", "Faqat .png, .jpg, .jpeg, .mp4 fayllarga ruxsat beriladi"));
            }
        }

        banner.Title = dto.Title;
        banner.Description = dto.Description;
        banner.IsActive = dto.IsActive;
        banner.UpdateDate = DateTime.UtcNow;

        _bannerRepository.Update(banner);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<BannerDto>.Success(MapToDto(banner));
    }


    public async Task<Result<bool>> DeleteBannerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
        if (banner is null)
            return Result<bool>.Failure(new Error("Banner.NotFound", "Banner topilmadi"));

        // Rasm faylini o‘chirish
        if (!string.IsNullOrEmpty(banner.MediaUrl))
        {
            var imagePath = Path.Combine("wwwroot", banner.MediaUrl.TrimStart('/'));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        _bannerRepository.Remove(banner);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> ToggleBannerStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
        if (banner is null)
            return Result<bool>.Failure(new Error("Banner.NotFound", "Banner topilmadi"));

        banner.IsActive = !banner.IsActive;
        banner.UpdateDate = DateTime.UtcNow;
        _bannerRepository.Update(banner);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }

    // Qo‘lda mapping funksiyasi
    private static BannerDto MapToDto(Banner banner)
    {
        return new BannerDto
        {
            Id = banner.Id,
            Title = banner.Title,
            Description = banner.Description,
            MediaUrl = banner.MediaUrl,
            IsActive = banner.IsActive
        };
    }
}
