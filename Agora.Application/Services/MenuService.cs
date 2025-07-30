using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Agora.Domain.Abstractions;
using Agora.Domain.Entities;
using Microsoft.AspNetCore.Http;
namespace Agora.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repo;
        private readonly IUnitOfWork _unit;

        public MenuService(IMenuRepository repo, IUnitOfWork unit)
        {
            _repo = repo;
            _unit = unit;
        }

        public async Task<Result<List<MenuDto>>> GetAllAsync()
        {
            try
            {
                var menus = await _repo.GetAllAsync();
                var menuDtos = menus
                    .OrderByDescending(menu => menu.CreateDate) // Eng yangi menyular birinchi
                    .Select(menu => new MenuDto
                    {
                        Id = menu.Id,
                        NameEn = menu.menuNameEn,
                        NameRu = menu.menuNameRu,
                        NameUz = menu.menuNameUz,
                        Category = menu.MenuCategory,
                        DescriptionEn = menu.DescriptionEn ?? "",
                        DescriptionRu = menu.DescriptionRu ?? "",
                        DescriptionUz = menu.DescriptionUz ?? "",
                        ImageUrl = menu.ImageUrl,
                        Price = menu.Price
                    }).ToList();

                return Result<List<MenuDto>>.Success(menuDtos);
            }
            catch (Exception ex)
            {
                return Result<List<MenuDto>>.Failure(new Error("Menu.GetAllError", ex.Message));
            }
        }

        public async Task<Result<MenuDto>> CreateAsync(CreateUpdateMenuDto dto)
        {
            try
            {
                var imageUrl = await SaveImageAsync(dto.Image);

                var menu = new Menu
                {
                    menuNameUz = dto.menuNameUz,
                    menuNameRu = dto.menuNameRu,
                    menuNameEn = dto.menuNameEn,
                    DescriptionUz = dto.DescriptionUz,
                    DescriptionRu = dto.DescriptionRu,
                    DescriptionEn = dto.DescriptionEn,
                    MenuCategory = dto.MenuCategory,
                    Price = dto.Price,
                    ImageUrl = imageUrl
                };

                await _repo.AddAsync(menu);
                await _unit.SaveChangesAsync();

                return Result<MenuDto>.Success(new MenuDto
                {
                    Id = menu.Id,
                    NameUz = menu.menuNameUz,
                    DescriptionUz = menu.DescriptionUz ?? "",
                    NameRu = menu.menuNameRu,
                    DescriptionRu = menu.DescriptionRu ?? "",
                    NameEn = menu.menuNameEn,
                    DescriptionEn = menu.DescriptionEn ?? "",
                    Category = menu.MenuCategory,
                    Price = menu.Price,
                    ImageUrl = menu.ImageUrl
                });
            }
            catch (Exception ex)
            {
                return Result<MenuDto>.Failure(new Error("Menu.CreateError", ex.Message));
            }
        }

        public async Task<Result<MenuDto>> UpdateAsync(Guid id, CreateUpdateMenuDto dto)
        {
            try
            {
                var menu = await _repo.GetByIdAsync(id);
                if (menu == null)
                    return Result<MenuDto>.Failure(new Error("Menu.NotFound", "Menu topilmadi"));

                // Agar yangi rasm yuklangan bo‘lsa, rasmni saqlash
                string? imageUrl = menu.ImageUrl; // Eski rasmni saqlab qolamiz
                if (dto.Image != null)
                {
                    // Eski rasmni o‘chirish (agar mavjud bo‘lsa)
                    if (!string.IsNullOrEmpty(menu.ImageUrl))
                    {
                        var oldImagePath = Path.Combine("wwwroot", menu.ImageUrl.TrimStart('/'));
                        if (File.Exists(oldImagePath))
                        {
                            File.Delete(oldImagePath);
                        }
                    }
                    // Yangi rasmni saqlash
                    imageUrl = await SaveImageAsync(dto.Image);
                }

                // Ma‘lumotlarni yangilash
                menu.menuNameUz = dto.menuNameUz;
                menu.menuNameEn = dto.menuNameEn;
                menu.menuNameRu = dto.menuNameRu;
                menu.DescriptionUz = dto.DescriptionUz;
                menu.DescriptionEn = dto.DescriptionEn;
                menu.DescriptionRu = dto.DescriptionRu;
                menu.MenuCategory = dto.MenuCategory;
                menu.Price = dto.Price;
                menu.ImageUrl = imageUrl;

                _repo.Update(menu);
                await _unit.SaveChangesAsync();

                return Result<MenuDto>.Success(new MenuDto
                {
                    Id = menu.Id,
                    NameUz = menu.menuNameUz,
                    DescriptionUz = menu.DescriptionUz ?? "",
                    NameRu = menu.menuNameRu,
                    DescriptionRu = menu.DescriptionRu ?? "",
                    NameEn = menu.menuNameEn,
                    DescriptionEn = menu.DescriptionEn ?? "",
                    Category = menu.MenuCategory,
                    Price = menu.Price,
                    ImageUrl = menu.ImageUrl
                });
            }
            catch (Exception ex)
            {
                return Result<MenuDto>.Failure(new Error("Menu.UpdateError", ex.Message));
            }
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var menu = await _repo.GetByIdAsync(id);
                if (menu == null)
                    return Result<bool>.Failure(new Error("Menu.NotFound", "Menu topilmadi"));

                // Rasm faylini o‘chirish
                if (!string.IsNullOrEmpty(menu.ImageUrl))
                {
                    var imagePath = Path.Combine("wwwroot", menu.ImageUrl.TrimStart('/'));
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                _repo.Remove(menu);
                await _unit.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(new Error("Menu.DeleteError", ex.Message));
            }
        }

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            try
            {
                var folder = "images/menus";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                var fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await image.CopyToAsync(stream);

                return $"/{folder}/{fileName}";
            }
            catch
            {
                // fallback path or rethrow depending on your architecture
                throw new Exception("Rasmni saqlashda xatolik yuz berdi.");
            }
        }
    }


}
