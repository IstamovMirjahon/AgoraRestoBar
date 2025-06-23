namespace Agora.Application.DTOs
{
    public class MenuDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public string Description { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
    }
}
