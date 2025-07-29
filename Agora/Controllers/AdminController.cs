using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        // GET /api/admin/contact
        [HttpGet("contact")]
        public async Task<IActionResult> GetContact()
        {
            var result = await _service.GetContactAsync();
            return result == null
                ? NotFound("Contact information not found.")
                : Ok(result);
        }

        // PUT /api/admin/contact
        [HttpPut("contact")]
        public async Task<IActionResult> UpdateContact([FromBody] ContactInfoDto dto)
        {
            await _service.UpdateContactAsync(dto);
            return NoContent();
        }

        // GET /api/admin/about
        [HttpGet("about")]
        public async Task<IActionResult> GetAbout()
        {
            var result = await _service.GetAboutAsync();
            return result == null
                ? NotFound("About information not found.")
                : Ok(result);
        }

        // PUT /api/admin/about
        [HttpPut("about")]
        public async Task<IActionResult> UpdateAbout([FromBody] AboutDto dto)
        {
            await _service.UpdateAboutAsync(dto);
            return NoContent();
        }

        // GET /api/admin/bookings
        [HttpGet("bookings")]
        public async Task<IActionResult> GetBookings()
        {
            var result = await _service.GetBookingsAsync();
            return result == null || !result.IsSuccess
                ? NotFound("No bookings found.")
                : Ok(result);
        }

        // PATCH /api/admin/bookings/{id}/confirm
        [HttpPatch("bookings/{bookingId}/confirm")]
        public async Task<IActionResult> ConfirmBooking([FromRoute] Guid bookingId, [FromBody] ConfirmBookingDto dto)
        {
            if (bookingId != dto.BookingId)
                return BadRequest("Booking ID mismatch.");

            await _service.ConfirmBookingAsync(dto.BookingId, dto.IsConfirmed);
            return NoContent();
        }
    }
}
