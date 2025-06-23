using Agora.Domain.Abstractions;
using Agora.Domain.Entities;

namespace Agora.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<Result<Admin>> GetAdminByUsernameAsync(string username);
        Task<Result<Admin>> UpdateRefreshTokenAsync(Guid adminId, string refreshToken, DateTime expiry);
        Task<Result<Admin>> GetAdminByRefreshTokenAsync(string refreshToken);
    }
}
