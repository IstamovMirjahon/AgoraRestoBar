
using Agora.Domain.Abstractions;

namespace Agora.Domain.Entities
{
    public class ContactInfo:BaseParams
    {
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

}
