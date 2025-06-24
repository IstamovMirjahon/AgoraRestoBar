
using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Agora.Domain.Entities;

namespace Agora.Application.Services
{
    public class AdminPanelService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminPanelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ContactInfoDto> GetContactAsync()
        {
            var entity = await _context.ContactInfos.FirstOrDefaultAsync();
            return new ContactInfoDto
            {
                Phone = entity?.Phone,
                Email = entity?.Email,
                Address = entity?.Address
            };
        }

        public async Task UpdateContactAsync(ContactInfoDto dto)
        {
            var entity = await _context.ContactInfos.FirstOrDefaultAsync();
            if (entity != null)
            {
                entity.Phone = dto.Phone;
                entity.Email = dto.Email;
                entity.Address = dto.Address;
            }
            else
            {
                entity = new ContactInfo
                {
                    Phone = dto.Phone,
                    Email = dto.Email,
                    Address = dto.Address
                };
                _context.ContactInfos.Add(entity);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<AboutDto> GetAboutAsync()
        {
            var entity = await _context.Abouts.FirstOrDefaultAsync();
            return new AboutDto { Content = entity?.Content };
        }

        public async Task UpdateAboutAsync(AboutDto dto)
        {
            var entity = await _context.Abouts.FirstOrDefaultAsync();
            if (entity != null)
            {
                entity.Content = dto.Content;
            }
            else
            {
                _context.Abouts.Add(new About { Content = dto.Content });
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Booking>> GetBookingsAsync()
        {
            return await _context.Bookings.OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task ConfirmBookingAsync(int bookingId, bool isConfirmed)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                booking.IsConfirmed = isConfirmed;
                await _context.SaveChangesAsync();
            }
        }
    }

}
