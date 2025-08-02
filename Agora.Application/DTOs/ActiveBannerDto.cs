namespace Agora.Application.DTOs
{
    public class ActiveBannerDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime UpdateDate { get; set; }
        public string MediaUrl { get; set; } = default!;
        public string MediaType { get; set; } = default!;
    }
}
