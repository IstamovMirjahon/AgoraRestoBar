using Agora.Domain.Abstractions;

namespace Agora.Domain.Entities
{
    public class Menu : BaseParams
    {
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = default!;
        public ICollection<MenuTranslation> Translations { get; set; } = new List<MenuTranslation>();
    }


}
