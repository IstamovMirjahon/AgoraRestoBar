using Microsoft.AspNetCore.Http;

namespace Agora.Application.DTOs
{
    public class CreateandUpdateMenuDto
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public string Description { get; set; } = default!;
        public IFormFile Image { get; set; } = default!;
    }
}
