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
        public async Task<IActionResult> GetContact()
        {
            var result = await _service.GetContactAsync();
            if (result == null)
            {
                return NotFound("Contact information not found.");
            }
            return Ok(result);
        }
        [HttpPut("contact")]
        public async Task<IActionResult> UpdateContact([FromBody] ContactInfoDto dto)
        {
            await _service.UpdateContactAsync(dto);

            return NoContent();
        }
        [HttpGet("about")]
        public async Task<IActionResult> GetAbout()
        {
            var result = await _service.GetAboutAsync();
            if (result == null)
            {
                return NotFound("About information not found.");
            }
            return Ok(result);
        }
            

        [HttpPut("about")]
        public async Task<IActionResult> UpdateAbout([FromBody] AboutDto dto)
        {
            await _service.UpdateAboutAsync(dto);
            return NoContent();
        }

        [HttpGet("bookings")]
        public async Task<IActionResult> GetBookings()
        {
            var result = await _service.GetBookingsAsync();
            if (result == null || !result.IsSuccess)
            {
                return NotFound("No bookings found.");
            }
            return Ok(result);
        }

        [HttpPost("bookings/confirm")]
        public async Task<IActionResult> ConfirmBooking([FromBody] ConfirmBookingDto dto)
        {
            await _service.ConfirmBookingAsync(dto.BookingId, dto.IsConfirmed);
            return NoContent();
        }
    }

}
