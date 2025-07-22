using Agora.Application.Interfaces;
using Agora.Domain.Abstractions;
using Agora.Domain.Entities;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Agora.Infrastructure.Repositories.Auth
{
    public class AuthRepository(
        ApplicationDbContext _context,
        IUnitOfWork _unitOfWork) : IAuthRepository
    {
        public async Task<Result<AdminProfile>> GetAdminByUsernameAsync(string username)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == username);

            if (admin is null || admin.IsDeleted)
                return Result<AdminProfile>.Failure(new("Admin.NotFound", "Admin not found or deleted."));

            return Result<AdminProfile>.Success(admin);
        }
        public async Task<Result<UserToken>> GetAdminByRefreshTokenAsync(string token)
        {
            var adminToken = await _context.UserTokens.FirstOrDefaultAsync(a => a.RefreshToken == token && a.RefreshTokenExpiration > DateTime.UtcNow);

            return adminToken == null
                ? Result<UserToken>.Failure(new("Token.Invalid", "Refresh token is invalid or expired."))
                : Result<UserToken>.Success(adminToken);
        }

        public async Task<Result<UserToken>> UpdateRefreshTokenAsync(Guid adminId, string token, DateTime expiry)
        {
            var admin = await _context.UserTokens.FindAsync(adminId);
            if (admin is null)
                return Result<UserToken>.Failure(new Error("Admin.NotFound", "Admin not found."));

            admin.RefreshToken = token;
            admin.RefreshTokenExpiration = expiry;
            _context.UserTokens.Update(admin);

            await _unitOfWork.SaveChangesAsync();
            return Result<UserToken>.Success(admin);
        }
        public async Task CreateOrUpdateToken(Guid userId, UserToken newToken)
        {
            var existingToken = await _context.UserTokens.FirstOrDefaultAsync(s => s.UserId == userId);

            if (existingToken != null)
            {
                existingToken.AccessToken = newToken.AccessToken;
                existingToken.AccessTokenExpiration = newToken.AccessTokenExpiration;
                existingToken.RefreshToken = newToken.RefreshToken;
                existingToken.RefreshTokenExpiration = newToken.RefreshTokenExpiration;
                _context.UserTokens.Update(existingToken);
            }
            else
            {
                await _context.UserTokens.AddAsync(newToken);
            }

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<Result<UserToken>> GetTokenByUserIdAsync(Guid userId)
        {
            var token = await _context.UserTokens.FirstOrDefaultAsync(t => t.UserId == userId);
            return token is null
                ? Result<UserToken>.Failure(new Error("Token.NotFound", "Token mavjud emas."))
                : Result<UserToken>.Success(token);
        }
        public async Task DeleteUserTokenAsync(Guid userId)
        {
            var token = await _context.UserTokens.FirstOrDefaultAsync(t => t.UserId == userId);
            if (token != null)
            {
                _context.UserTokens.Remove(token);
                await _unitOfWork.SaveChangesAsync();
            }
        }

    }
}
