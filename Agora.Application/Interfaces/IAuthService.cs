using Agora.Application.DTOs;
using Agora.Domain.Abstractions;

namespace Agora.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResponseDto>> LoginAsync(AdminLoginDto dto);
        Task<Result<AuthResponseDto>> RefreshTokenAsync(string refreshToken);
        Task<Result<bool>> LogoutAsync(Guid userId);
    }
}
