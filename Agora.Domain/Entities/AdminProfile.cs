using Agora.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Agora.Domain.Entities
{
    public class AdminProfile : BaseParams
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
