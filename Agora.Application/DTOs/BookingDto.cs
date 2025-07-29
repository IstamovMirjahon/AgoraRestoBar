namespace Agora.Application.DTOs
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public DateTime ReservationTime { get; set; }
        public bool IsConfirmed { get; set; }
    }

}
