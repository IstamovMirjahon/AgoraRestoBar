using Agora.Domain.Abstractions;

namespace Agora.Domain.Entities
{
    public class Banner : BaseParams
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public bool IsActive { get; set; } = true;
    }
}
