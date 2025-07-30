using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // POST /api/bookings
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto, CancellationToken cancellationToken)
        {
            var result = await _bookingService.CreateAsync(dto, cancellationToken);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error?.Message);
            }

            return Ok(new { message = "Buyurtmangiz qabul qilindi!" });
        }

        // GET /api/bookings
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _bookingService.GetAllAsync(cancellationToken);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error?.Message);
            }

            return Ok(result.Value);
        }
        // PUT /api/bookings/{id}/toggle-confirmation
        [HttpPut("{id}/toggle-confirmation")]
        public async Task<IActionResult> ToggleConfirmation(Guid id, CancellationToken cancellationToken)
        {
            var result = await _bookingService.ToggleConfirmationAsync(id, cancellationToken);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error?.Message);
            }

            return Ok(new { message = "Booking confirmation status toggled successfully." });
        }
    }
}
