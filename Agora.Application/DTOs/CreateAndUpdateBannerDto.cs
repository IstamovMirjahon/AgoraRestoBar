using Microsoft.AspNetCore.Http;

namespace Agora.Application.DTOs
{
    public class CreateAndUpdateBannerDto
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public IFormFile? MediaUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
