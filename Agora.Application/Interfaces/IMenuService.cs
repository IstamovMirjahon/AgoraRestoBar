using Agora.Application.DTOs;
using Agora.Domain.Abstractions;

namespace Agora.Application.Interfaces
{
    public interface IMenuService
    {
        Task<Result<List<MenuDto>>> GetAllAsync();
        Task<Result<MenuDto>> CreateAsync(CreateUpdateMenuDto dto);
        Task<Result<MenuDto>> UpdateAsync(Guid id, CreateUpdateMenuDto dto);
        Task<Result<bool>> DeleteAsync(Guid id);
    }

}
