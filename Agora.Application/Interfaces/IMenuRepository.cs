using Agora.Domain.Entities;

namespace Agora.Application.Interfaces
{
    public interface IMenuRepository
    {
        Task<List<Menu>> GetAllAsync();
        Task<List<Menu>> GetAllWithTranslationsAsync(string languageCode);
        Task<Menu?> GetByIdAsync(Guid id);
        Task<Menu?> GetByIdWithTranslationsAsync(Guid id, string? languageCode = null);
        Task AddAsync(Menu menu);
        void Update(Menu menu);
        void Remove(Menu menu);
    }

}
