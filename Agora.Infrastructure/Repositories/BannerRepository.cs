using Agora.Application.Interfaces;
using Agora.Domain.Entities;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories
{
    public class BannerRepository : IBannerRepository
    {
        private readonly ApplicationDbContext _context;

        public BannerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Banner>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _context.Banners.ToListAsync(cancellationToken);

        public async Task<Banner?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await _context.Banners.FindAsync(new object[] { id }, cancellationToken);

        public async Task AddAsync(Banner banner, CancellationToken cancellationToken = default)
            => await _context.Banners.AddAsync(banner, cancellationToken);

        public void Update(Banner banner)
            => _context.Banners.Update(banner);

        public void Remove(Banner banner)
            => _context.Banners.Remove(banner);
    }
}
