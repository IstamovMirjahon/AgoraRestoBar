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

        public async Task<Menu?> GetByIdAsync(Guid id)
        {
            return await _context.Menus.FindAsync(id);
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
