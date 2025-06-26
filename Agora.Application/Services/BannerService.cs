using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Agora.Domain.Abstractions;
using Agora.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Agora.Application.Services
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BannerService(IBannerRepository bannerRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _bannerRepository = bannerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BannerDto>> GetAllBannersAsync(CancellationToken cancellationToken = default)
        {
            var banners = await _bannerRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<BannerDto>>(banners);
        }

        public async Task<BannerDto?> GetBannerByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
            return banner is null ? null : _mapper.Map<BannerDto>(banner);
        }

        public async Task<BannerDto> CreateBannerAsync(CreateAndUpdateBannerDto dto, CancellationToken cancellationToken = default)
        {
            string imagePath = await SaveImageAsync(dto.Image);
            var banner = new Banner
            {
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = imagePath,
                IsActive = dto.IsActive,
                CreateDate = DateTime.UtcNow
            };

            await _bannerRepository.AddAsync(banner, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<BannerDto>(banner);
        }

        public async Task<BannerDto> UpdateBannerAsync(Guid id, CreateAndUpdateBannerDto dto, CancellationToken cancellationToken = default)
        {
            var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
            if (banner is null) throw new Exception("Banner not found");

            banner.Title = dto.Title;
            banner.Description = dto.Description;
            banner.IsActive = dto.IsActive;
            if (dto.Image != null)
                banner.ImageUrl = await SaveImageAsync(dto.Image);

            _bannerRepository.Update(banner);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<BannerDto>(banner);
        }

        public async Task<bool> DeleteBannerAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
            if (banner is null) return false;
            _bannerRepository.Remove(banner);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> ToggleBannerStatusAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
            if (banner is null) return false;
            banner.IsActive = !banner.IsActive;
            _bannerRepository.Update(banner);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            string folder = Path.Combine("wwwroot", "images", "banners");
            Directory.CreateDirectory(folder);
            string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            string filePath = Path.Combine(folder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);
            return $"/images/banners/{fileName}";
        }
    }
}
