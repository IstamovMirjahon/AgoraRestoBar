
using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Agora.Domain.Abstractions;
using Agora.Domain.Entities;

namespace Agora.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repo;

        public AdminService(IAdminRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<ContactInfoDto>> GetContactAsync()
        {
            var entity = await _repo.GetContactAsync();
            if (entity == null)
                return Result<ContactInfoDto>.Failure(new Error("Contact.NotFound", "Contact information not found."));

           

            return Result<ContactInfoDto>.Success(new ContactInfoDto
            {
                Phone = entity.Phone,
                Email = entity.Email,
                Address = entity.Address

            });
        }




        public async Task UpdateContactAsync(ContactInfoDto dto)
        {
            var model = new ContactInfo
            {
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address
            };
            await _repo.UpdateContactAsync(model);
            await _repo.SaveChangesAsync();
        }

        public async Task<Result<AboutDto>> GetAboutAsync()
        {
            var entity = await _repo.GetAboutAsync();
            if (entity is null)
                return Result<AboutDto>.Failure(new Error("About.NotFound", "About information not found."));
            var dto = new AboutDto()
            {
                Content = entity.Content
            };
            return Result<AboutDto>.Success(dto);




        }

        public async Task UpdateAboutAsync(AboutDto dto)
        {
            var model = new About
            {
                Content = dto.Content
            };
            await _repo.UpdateAboutAsync(model);
            await _repo.SaveChangesAsync();
        }

        public async Task<Result<List<Booking>>> GetBookingsAsync()
        {
            var top = await _repo.GetAllBookingsAsync();
          if(top is null)
                return Result<List<Booking>>.Failure(new Error("Booking.NotFound", "No bookings found."));
            return Result<List<Booking>>.Success(top);
        }

        public async Task ConfirmBookingAsync(int id, bool isConfirmed)
        {
            var booking = await _repo.GetBookingByIdAsync(id);
            if (booking != null)
            {
                booking.IsConfirmed = isConfirmed;
                await _repo.SaveChangesAsync();
            }
        }
    }

}
