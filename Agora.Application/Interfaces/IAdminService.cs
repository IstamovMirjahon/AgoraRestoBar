using Agora.Application.DTOs;
using Agora.Domain.Abstractions;
using Agora.Domain.Entities;

namespace Agora.Application.Interfaces
{
    public interface IAdminService
    {
        Task<Result<ContactInfoDto>> GetContactAsync();
        Task UpdateContactAsync(ContactInfoDto dto);

        Task<Result<AboutDto>> GetAboutAsync();
        Task UpdateAboutAsync(AboutDto dto);

        Task<Result<List<Booking>>> GetBookingsAsync();
        Task ConfirmBookingAsync(int id, bool isConfirmed);
    }


}
