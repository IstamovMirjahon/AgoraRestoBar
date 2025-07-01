using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Agora.Domain.Abstractions;
using Agora.Domain.Entities;

namespace Agora.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
        }

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
    }
}
