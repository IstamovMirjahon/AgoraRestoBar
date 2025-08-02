using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Agora.Domain.Abstractions;
using Agora.Domain.Entities;
using AutoMapper;

namespace Agora.Application.Services
{
    public class BookingService
        (IBookingRepository _bookingRepository,
        IUnitOfWork _unitOfWork,
        IMapper _mapper) : IBookingService
    {
        public async Task<Result<bool>> CreateAsync(CreateBookingDto dto, CancellationToken cancellationToken = default)
        {
            if(string.IsNullOrWhiteSpace(dto.FullName) || string.IsNullOrWhiteSpace(dto.Phone))
            {
                return Result<bool>.Failure(new Error("ValidationError", "Full name and phone number must not be empty."));
            }

            var booking = new Booking
            {
                FullName = dto.FullName,
                Phone = dto.Phone,
                IsConfirmed = false
            };

            await _bookingRepository.AddAsync(booking, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }

        public async Task<Result<List<BookingDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var bookings = await _bookingRepository.GetAllAsync();

            if (bookings is null)
            {
                return Result<List<BookingDto>>.Failure(new Error("Booking.NotFound", "No bookings found."));
            }

            // 🔽 Oxirgi qo‘shilganlar tepada bo‘lishi uchun tartiblash
            var orderedBookings = bookings
                .OrderByDescending(b => b.CreateDate) // yoki b.Id agar CreatedAt yo‘q bo‘lsa
                .ToList();

            var bookingDtos = _mapper.Map<List<BookingDto>>(orderedBookings);

            return Result<List<BookingDto>>.Success(bookingDtos);
        }

        public async Task<Result<List<BookingDto>>> GetUnconfirmedAllAsync(CancellationToken cancellationToken = default)
        {
            var bookings = await _bookingRepository.GetAllAsync();

            if (bookings is null)
            {
                return Result<List<BookingDto>>.Failure(new Error("Booking.NotFound", "No bookings found."));
            }
            // Faqat tasdiqlanmaganlarni olish
            var unconfirmedBookings = bookings
                .Where(b => !b.IsConfirmed)
                .OrderByDescending(b => b.CreateDate) 
                .ToList();
            var bookingDtos = _mapper.Map<List<BookingDto>>(unconfirmedBookings);
            return Result<List<BookingDto>>.Success(bookingDtos);
        }

        public async Task<Result<bool>> ToggleConfirmationAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);
            if (booking == null)
            {
                return Result<bool>.Failure(new Error("Booking.NotFound", "Booking not found."));
            }

            booking.IsConfirmed = !booking.IsConfirmed;
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }

    }
}
