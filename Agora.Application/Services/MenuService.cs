using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Agora.Domain.Abstractions;
using Agora.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.Application.Services
{
   public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MenuService(IMenuRepository menuRepository, IUnitOfWork unitOfWork)
    {
        _menuRepository = menuRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<MenuDto>>> GetAllAsync(string languageCode)
    {
        var menus = await _menuRepository.GetAllWithTranslationsAsync(languageCode);
        var result = menus.Select(m => new MenuDto
        {
            Id = m.Id,
            Price = m.Price,
            ImageUrl = m.ImageUrl,
            Name = m.Translations.FirstOrDefault()?.Name ?? "",
            Description = m.Translations.FirstOrDefault()?.Description ?? ""
        }).ToList();

        return Result<List<MenuDto>>.Success(result);
    }

    public async Task<Result<MenuDto>> GetByIdAsync(Guid id, string languageCode)
    {
        var menu = await _menuRepository.GetByIdWithTranslationsAsync(id, languageCode);
        if (menu == null)
            return Result<MenuDto>.Failure(new Error("Menu.NotFound", "Menu topilmadi."));

        var translation = menu.Translations.FirstOrDefault();

        return Result<MenuDto>.Success(new MenuDto
        {
            Id = menu.Id,
            Price = menu.Price,
            ImageUrl = menu.ImageUrl,
            Name = translation?.Name ?? "",
            Description = translation?.Description ?? ""
        });
    }

    public async Task<Result<MenuDto>> CreateAsync(CreateandUpdateMenuDto dto)
    {
        string imageUrl = await SaveImageAsync(dto.Image);

        var menu = new Menu
        {
            Price = dto.Price,
            ImageUrl = imageUrl,
            Translations = dto.Translations.Select(t => new MenuTranslation
            {
                LanguageCode = t.LanguageCode,
                Name = t.Name,
                Description = t.Description
            }).ToList()
        };

        await _menuRepository.AddAsync(menu);
        await _unitOfWork.SaveChangesAsync();

        return Result<MenuDto>.Success(new MenuDto
        {
            Id = menu.Id,
            Price = menu.Price,
            ImageUrl = menu.ImageUrl
        });
    }

    public async Task<Result<MenuDto>> UpdateAsync(Guid id, CreateandUpdateMenuDto dto)
    {
        var menu = await _menuRepository.GetByIdWithTranslationsAsync(id);
        if (menu == null)
            return Result<MenuDto>.Failure(new Error("Menu.NotFound", "Menu topilmadi."));

        string imageUrl = await SaveImageAsync(dto.Image);

        menu.Price = dto.Price;
        menu.ImageUrl = imageUrl;

        menu.Translations = dto.Translations.Select(t => new MenuTranslation
        {
            MenuId = menu.Id,
            LanguageCode = t.LanguageCode,
            Name = t.Name,
            Description = t.Description
        }).ToList();

        _menuRepository.Update(menu);
        await _unitOfWork.SaveChangesAsync();

        return Result<MenuDto>.Success(new MenuDto
        {
            Id = menu.Id,
            Price = menu.Price,
            ImageUrl = menu.ImageUrl
        });
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        var menu = await _menuRepository.GetByIdAsync(id);
        if (menu == null)
            return Result<bool>.Failure(new Error("Menu.NotFound", "Menu topilmadi."));

        _menuRepository.Remove(menu);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    private async Task<string> SaveImageAsync(IFormFile image)
    {
        var folderName = "images/menus";
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
        var filePath = Path.Combine(uploadPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        return $"/{folderName}/{fileName}";
    }
}
}
