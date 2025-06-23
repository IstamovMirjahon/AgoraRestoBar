using Agora.Domain.Entities;

namespace Agora.Application.Interfaces
{
    public interface IMenuRepository
    {
        Task<List<Menu>> GetAllAsync();
        Task<Menu?> GetByIdAsync(Guid id);
        Task AddAsync(Menu menu);
        void Update(Menu menu);
        void Remove(Menu menu);
    }
}
