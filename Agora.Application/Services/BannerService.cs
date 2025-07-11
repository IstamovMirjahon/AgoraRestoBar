﻿using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Agora.Domain.Abstractions;
using Agora.Domain.Entities;
using AutoMapper;

namespace Agora.Application.Services;

public class BannerService : IBannerService
{
    private readonly IBannerRepository _bannerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public BannerService(
        IBannerRepository bannerRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IFileService fileService)
    {
        _bannerRepository = bannerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
    }
    public async Task<Result<List<BannerDto>>> GetAllBannersAsync(CancellationToken cancellationToken = default)
    {
        var banners = await _bannerRepository.GetAllAsync(cancellationToken);
        var result = _mapper.Map<List<BannerDto>>(banners);
        return Result<List<BannerDto>>.Success(result);
    }
    public async Task<Result<BannerDto>> GetBannerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
        if (banner is null)
            return Result<BannerDto>.Failure(new Error("Banner.NotFound", "Banner topilmadi"));

        var dto = _mapper.Map<BannerDto>(banner);
        return Result<BannerDto>.Success(dto);
    }
    public async Task<Result<BannerDto>> CreateBannerAsync(CreateAndUpdateBannerDto dto, CancellationToken cancellationToken = default)
    {
        var imagePath = await _fileService.SaveImageAsync(dto.Image, "banners", cancellationToken);

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

        return Result<BannerDto>.Success(_mapper.Map<BannerDto>(banner));
    }
    public async Task<Result<BannerDto>> UpdateBannerAsync(Guid id, CreateAndUpdateBannerDto dto, CancellationToken cancellationToken = default)
    {
        var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
        if (banner is null)
            return Result<BannerDto>.Failure(new Error("Banner.NotFound", "Banner topilmadi"));

        banner.Title = dto.Title;
        banner.Description = dto.Description;
        banner.IsActive = dto.IsActive;

        if (dto.Image is not null)
            banner.ImageUrl = await _fileService.SaveImageAsync(dto.Image, "banners", cancellationToken);

        _bannerRepository.Update(banner);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<BannerDto>.Success(_mapper.Map<BannerDto>(banner));
    }

    public async Task<Result<bool>> DeleteBannerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
        if (banner is null)
            return Result<bool>.Failure(new Error("Banner.NotFound", "Banner topilmadi"));

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
        _bannerRepository.Update(banner);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
