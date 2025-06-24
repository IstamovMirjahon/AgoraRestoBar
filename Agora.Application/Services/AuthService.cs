using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Agora.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Agora.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }
        public async Task<AuthResponseDto> LoginAsync(AdminLoginDto dto)
        {
            var admin = await _authRepository.GetAdminByUsernameAsync(dto.Username);
            if (admin == null || Hash(dto.Password) != admin.Value?.PasswordHash)
                throw new UnauthorizedAccessException("Login yoki parol noto‘g‘ri");

            var accessToken = GenerateJwtToken(admin.Value);
            var refreshToken = GenerateRefreshToken();

            await _authRepository.UpdateRefreshTokenAsync(admin.Value.Id, refreshToken, DateTime.UtcNow.AddDays(7));

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var admin = await _authRepository.GetAdminByRefreshTokenAsync(refreshToken);
            if (admin.Value == null)
                throw new UnauthorizedAccessException("Refresh token noto‘g‘ri yoki muddati tugagan");

            var newAccessToken = GenerateJwtToken(admin.Value);
            var newRefreshToken = GenerateRefreshToken();

            await _authRepository.UpdateRefreshTokenAsync(admin.Value.Id, newRefreshToken, DateTime.UtcNow.AddDays(7));

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
        // JWT yaratish
        private string GenerateJwtToken(AdminProfile admin)
        {
            var keyString = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new InvalidOperationException("JWT key is missing in configuration.");

            var claims = new[]
            {
             new Claim(ClaimTypes.Name, admin.Username),
             new Claim("role", "admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
        private string Hash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }
    }
}
