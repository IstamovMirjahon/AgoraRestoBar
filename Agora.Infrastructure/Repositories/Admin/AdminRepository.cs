
using Agora.Application.Interfaces;
using Agora.Domain.Abstractions;
using Agora.Domain.Entities;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories.Admin
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ContactInfo> GetContactAsync()
        {
            var top= await _context.ContactInfos.FirstOrDefaultAsync();
            if (top == null)
            {
                throw new KeyNotFoundException("Contact information not found.");
            }
            return top;
        }

        public async Task UpdateContactAsync(ContactInfo contact)
        {
            var existing = await _context.ContactInfos.FirstOrDefaultAsync();
            if (existing != null)
            {
                existing.Phone = contact.Phone;
                existing.Email = contact.Email;
                existing.Address = contact.Address;
            }
            else
            {
                _context.ContactInfos.Add(contact);
            }
        }

        public async Task<About> GetAboutAsync()
        {
            var top = await _context.Abouts.FirstOrDefaultAsync();
            if (top == null)
            {
                throw new KeyNotFoundException("About information not found.");
            }
            return top;


        }

        public async Task UpdateAboutAsync(About about)
        {
            var existing = await _context.Abouts.FirstOrDefaultAsync();
            if (existing != null)
            {
                existing.Content = about.Content;
            }
            else
            {
                _context.Abouts.Add(about);
            }
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings.OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            var existing = await _context.Bookings.FindAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Booking with ID {id} not found.");
            }
            return existing;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
