using Agora.Domain.Abstractions;
using Agora.Domain.Entities;

namespace Agora.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<Result<AdminProfile>> GetAdminByUsernameAsync(string username);
        Task<Result<AdminProfile>> UpdateRefreshTokenAsync(Guid adminId, string refreshToken, DateTime expiry);
        Task<Result<AdminProfile>> GetAdminByRefreshTokenAsync(string refreshToken);
    }
}
