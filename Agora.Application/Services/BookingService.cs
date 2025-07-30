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
            var booking = new Booking
            {
                FullName = dto.FullName,
                Phone = dto.Phone,
                ReservationTime = DateTime.UtcNow,
                IsConfirmed = false,
                CreateDate = DateTime.UtcNow
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
            var bookingDtos =  _mapper.Map<List<BookingDto>>(bookings);


            return Result<List<BookingDto>>.Success(bookingDtos);
        }
    }
}
