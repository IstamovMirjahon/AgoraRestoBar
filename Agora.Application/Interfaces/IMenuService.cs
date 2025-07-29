using Agora.Application.DTOs;
using Agora.Domain.Abstractions;

namespace Agora.Application.Interfaces
{
    public interface IMenuService
    {
        Task<Result<List<MenuDto>>> GetAllAsync(string languageCode);
        Task<Result<MenuDto>> GetByIdAsync(Guid id, string languageCode);
        Task<Result<MenuDto>> CreateAsync(CreateandUpdateMenuDto dto);
        Task<Result<MenuDto>> UpdateAsync(Guid id, CreateandUpdateMenuDto dto);
        Task<Result<bool>> DeleteAsync(Guid id);
    }

}
