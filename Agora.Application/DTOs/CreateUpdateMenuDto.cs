using Microsoft.AspNetCore.Http;

namespace Agora.Application.DTOs
{
   public class CreateUpdateMenuDto
{
        public string menuNameUz { get; set; } = default!;
        public string menuNameEn { get; set; } = default!;
        public string menuNameRu { get; set; } = default!;
        public string? DescriptionUz { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionRu { get; set; }
        public string MenuCategory { get; set; } = default!;
        public decimal Price { get; set; }
        public IFormFile? Image { get; set; }
    }
}
