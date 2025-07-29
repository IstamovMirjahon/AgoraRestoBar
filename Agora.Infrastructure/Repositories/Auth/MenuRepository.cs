using Agora.Application.Interfaces;
using Agora.Domain.Entities;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories.Auth
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Menu>> GetAllAsync()
        {
            return await _context.Menus.ToListAsync();
        }

        public async Task<List<Menu>> GetAllWithTranslationsAsync(string languageCode)
        {
            return await _context.Menus
                .Include(m => m.Translations.Where(t => t.LanguageCode == languageCode))
                .ToListAsync();
        }

        public async Task<Menu?> GetByIdAsync(Guid id)
        {
            return await _context.Menus.FindAsync(id);
        }

        public async Task<Menu?> GetByIdWithTranslationsAsync(Guid id, string? languageCode = null)
        {
            var query = _context.Menus
                .Include(m => m.Translations)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                query = query
                    .Where(m => m.Id == id)
                    .Select(m => new Menu
                    {
                        Id = m.Id,
                        Price = m.Price,
                        ImageUrl = m.ImageUrl,
                        Translations = m.Translations
                            .Where(t => t.LanguageCode == languageCode)
                            .ToList()!
                    });
            }
            else
            {
                query = query.Where(m => m.Id == id);
            }

            return await query.FirstOrDefaultAsync();
        }


        public async Task AddAsync(Menu menu)
        {
            await _context.Menus.AddAsync(menu);
        }

        public void Update(Menu menu)
        {
            _context.Menus.Update(menu);
        }

        public void Remove(Menu menu)
        {
            _context.Menus.Remove(menu);
        }
    }
}
