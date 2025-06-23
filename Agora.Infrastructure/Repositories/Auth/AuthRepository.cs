using Agora.Application.Interfaces;
using Agora.Domain.Abstractions;
using Agora.Domain.Entities;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public AuthRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Admin>> GetAdminByUsernameAsync(string username)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == username);
            if (admin is null)
                return Result<Admin>.Failure(new Error("Admin.NotFound", "Admin not found."));

            return Result<Admin>.Success(admin);
        }
        public async Task<Result<Admin>> GetAdminByRefreshTokenAsync(string token)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a =>
                a.RefreshToken == token && a.RefreshTokenExpiryTime > DateTime.UtcNow);

            return admin is null
                ? Result<Admin>.Failure(new Error("Token.Invalid", "Refresh token is invalid or expired."))
                : Result<Admin>.Success(admin);
        }

        public async Task<Result<Admin>> UpdateRefreshTokenAsync(Guid adminId, string token, DateTime expiry)
        {
            var admin = await _context.Admins.FindAsync(adminId);
            if (admin is null)
                return Result<Admin>.Failure(new Error("Admin.NotFound", "Admin not found."));

            admin.RefreshToken = token;
            admin.RefreshTokenExpiryTime = expiry;
            await _unitOfWork.SaveChangesAsync();
            return Result<Admin>.Success(admin);
        }
    }
}
