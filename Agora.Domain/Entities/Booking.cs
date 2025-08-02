
using Agora.Domain.Abstractions;

namespace Agora.Domain.Entities
{
    public class Booking : BaseParams
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public bool IsConfirmed { get; set; } = false;
    }

}
