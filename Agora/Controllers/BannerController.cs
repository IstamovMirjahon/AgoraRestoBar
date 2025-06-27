using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Route("api/admin/banner")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var banners = await _bannerService.GetAllBannersAsync(cancellationToken);
            return Ok(banners);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var banner = await _bannerService.GetBannerByIdAsync(id, cancellationToken);
            if (banner is null) return NotFound();
            return Ok(banner);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateAndUpdateBannerDto dto, CancellationToken cancellationToken)
        {
            var banner = await _bannerService.CreateBannerAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = banner.Value?.Id }, banner);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] CreateAndUpdateBannerDto dto, CancellationToken cancellationToken)
        {
            var banner = await _bannerService.UpdateBannerAsync(id, dto, cancellationToken);
            return Ok(banner);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _bannerService.DeleteBannerAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : NotFound(result.Error?.Message);
        }

        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(Guid id, CancellationToken cancellationToken)
        {
            var result = await _bannerService.ToggleBannerStatusAsync(id, cancellationToken);
            return result.IsSuccess ? Ok() : NotFound(result.Error?.Message);
        }

    }

}
