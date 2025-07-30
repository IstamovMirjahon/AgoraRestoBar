namespace Agora.Application.DTOs
{
    public class MenuDto
    {
        public Guid Id { get; set; }
        public string NameUz { get; set; } = default!;
        public string NameRu { get; set; } = default!;
        public string NameEn { get; set; } = default!;
        public string? DescriptionUz { get; set; }
        public string? DescriptionRu { get; set; }
        public string? DescriptionEn { get; set; }
        public string? Category { get; set; } = default!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = default!;
    }
}
