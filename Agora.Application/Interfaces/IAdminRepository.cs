
using Agora.Domain.Abstractions;
using Agora.Domain.Entities;

namespace Agora.Application.Interfaces
{
    public interface IAdminRepository
    {
        Task<ContactInfo> GetContactAsync();
        Task UpdateContactAsync(ContactInfo contact);

        Task<About> GetAboutAsync();
        Task UpdateAboutAsync(About about);

        Task<List<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(Guid id);
        Task SaveChangesAsync();
    }


}
