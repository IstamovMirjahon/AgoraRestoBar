using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Authorize]
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        [HttpGet("contact")]
        public async Task<IActionResult> GetContact() =>
            Ok(await _service.GetContactAsync());

        [HttpPut("contact")]
        public async Task<IActionResult> UpdateContact([FromBody] ContactInfoDto dto)
        {
            await _service.UpdateContactAsync(dto);
            return NoContent();
        }

        [HttpGet("about")]
        public async Task<IActionResult> GetAbout() =>
            Ok(await _service.GetAboutAsync());

        [HttpPut("about")]
        public async Task<IActionResult> UpdateAbout([FromBody] AboutDto dto)
        {
            await _service.UpdateAboutAsync(dto);
            return NoContent();
        }

        [HttpGet("bookings")]
        public async Task<IActionResult> GetBookings() =>
            Ok(await _service.GetBookingsAsync());

        [HttpPost("bookings/confirm")]
        public async Task<IActionResult> ConfirmBooking([FromBody] ConfirmBookingDto dto)
        {
            await _service.ConfirmBookingAsync(dto.BookingId, dto.IsConfirmed);
            return NoContent();
        }
    }

}
