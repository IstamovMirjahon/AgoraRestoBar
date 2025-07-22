using Agora.Application.DTOs;
using Agora.Application.DTOs.Errors;
using Agora.Application.Interfaces;
using Agora.Domain.Abstractions;
using Agora.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Agora.Application.Services
{
    public class AuthService(
        IAuthRepository _authRepository,
        IConfiguration _configuration) : IAuthService
    {
        public async Task<Result<AuthResponseDto>> LoginAsync(AdminLoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return Result<AuthResponseDto>.Failure(new("Auth.InvalidInput", "Username or password is required."));

            var adminResult = await _authRepository.GetAdminByUsernameAsync(dto.Username);
            if (adminResult.IsFailure || adminResult.Value == null)
                return Result<AuthResponseDto>.Failure(new("Auth.Invalid", "Login yoki parol noto‘g‘ri"));

            if (Hash(dto.Password) != adminResult.Value.PasswordHash)
                return Result<AuthResponseDto>.Failure(new("Auth.Invalid", "Login yoki parol noto‘g‘ri"));

            var userToken = GenerateUserToken(adminResult.Value.Id);

            await _authRepository.CreateOrUpdateToken(adminResult.Value.Id, userToken);

            return Result<AuthResponseDto>.Success(new AuthResponseDto
            {
                AccessToken = userToken.AccessToken,
                RefreshToken = userToken.RefreshToken
            });
        }
        public async Task<Result<bool>> LogoutAsync(Guid userId)
        {
            var token = await _authRepository.GetTokenByUserIdAsync(userId);
            if (token.IsFailure || token.Value == null)
                return Result<bool>.Failure(new Error("Logout.NotFound", "Token topilmadi."));

            await _authRepository.DeleteUserTokenAsync(userId);
            return Result<bool>.Success(true);
        }

        public async Task<Result<AuthResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            var tokenResult = await _authRepository.GetAdminByRefreshTokenAsync(refreshToken);
            if (tokenResult.IsFailure || tokenResult.Value == null)
                return Result<AuthResponseDto>.Failure(new("Token.Invalid", "Refresh token is invalid or expired."));

            var newToken = GenerateUserToken(tokenResult.Value.UserId);
            await _authRepository.CreateOrUpdateToken(tokenResult.Value.UserId, newToken);

            return Result<AuthResponseDto>.Success(new AuthResponseDto
            {
                AccessToken = newToken.AccessToken,
                RefreshToken = newToken.RefreshToken
            });
        }
        // JWT yaratish
        private string GenerateJwtToken(Guid userId)
        {
            var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is missing in configuration.");
            var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("userid", userId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private string Hash(string password)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
        private UserToken GenerateUserToken(Guid userId)
        {
            return new UserToken
            {
                UserId = userId,
                AccessToken = GenerateJwtToken(userId),
                AccessTokenExpiration = DateTime.UtcNow.AddHours(10),
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpiration = DateTime.UtcNow.AddHours(11)
            };
        }
    }
}
