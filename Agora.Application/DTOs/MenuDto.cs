namespace Agora.Application.DTOs
{
    public class MenuDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
