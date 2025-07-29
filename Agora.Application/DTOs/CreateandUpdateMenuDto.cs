using Microsoft.AspNetCore.Http;

namespace Agora.Application.DTOs
{
   public class CreateandUpdateMenuDto
{
    public decimal Price { get; set; }
    public IFormFile Image { get; set; } = default!;
    public List<MenuTranslationDto> Translations { get; set; } = new();
}
}
