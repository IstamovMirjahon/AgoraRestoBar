using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Route("booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto, CancellationToken cancellationToken)
        {
            var result = await _bookingService.CreateAsync(dto, cancellationToken);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error?.Message);
            }
            return result.IsSuccess ? Ok(new { message = "Buyurtmangiz qabul qilindi!" }) : BadRequest(result.Error?.Message);
        }
    }
}
