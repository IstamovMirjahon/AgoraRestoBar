using Agora.Domain.Abstractions;
using Agora.Domain.Entities;

namespace Agora.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<Result<AdminProfile>> GetAdminByUsernameAsync(string username);
        Task<Result<UserToken>> UpdateRefreshTokenAsync(Guid adminId, string refreshToken, DateTime expiry);
        Task<Result<UserToken>> GetAdminByRefreshTokenAsync(string refreshToken);
        Task CreateOrUpdateToken(Guid id, UserToken newUserToken);
        Task<Result<UserToken>> GetTokenByUserIdAsync(Guid userId);
        Task DeleteUserTokenAsync(Guid userId);
    }
}
