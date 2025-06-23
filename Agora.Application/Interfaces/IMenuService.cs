using Agora.Application.DTOs;
using Agora.Domain.Abstractions;

namespace Agora.Application.Interfaces
{
    public interface IMenuService
    {
        Task<Result<List<MenuDto>>> GetAllAsync();
        Task<Result<MenuDto>> GetByIdAsync(Guid id);
        Task<Result<MenuDto>> CreateAsync(CreateandUpdateMenuDto dto);
        Task<Result<MenuDto>> UpdateAsync(Guid id, CreateandUpdateMenuDto dto);
        Task<Result<bool>> DeleteAsync(Guid id);
    }

}
