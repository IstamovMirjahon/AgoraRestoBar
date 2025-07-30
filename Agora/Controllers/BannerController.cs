using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/admin/banners")]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        // GET /api/admin/banners
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var banners = await _bannerService.GetAllBannersAsync(cancellationToken);
            return !banners.IsSuccess
                ? BadRequest(banners.Error?.Message)
                : Ok(banners);
        }

        // GET /api/admin/banners/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var banner = await _bannerService.GetBannerByIdAsync(id, cancellationToken);
            return banner == null ? NotFound() : Ok(banner);
        }

        // POST /api/admin/banners
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateAndUpdateBannerDto dto, CancellationToken cancellationToken)
        {
            var banner = await _bannerService.CreateBannerAsync(dto, cancellationToken);
            return !banner.IsSuccess
                ? BadRequest(banner.Error?.Message)
                : CreatedAtAction(nameof(GetById), new { id = banner.Value?.Id }, banner);
        }

        // PUT /api/admin/banners/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] CreateAndUpdateBannerDto dto, CancellationToken cancellationToken)
        {
            var banner = await _bannerService.UpdateBannerAsync(id, dto, cancellationToken);
            return !banner.IsSuccess
                ? BadRequest(banner.Error?.Message)
                : Ok(banner);
        }

        // DELETE /api/admin/banners/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _bannerService.DeleteBannerAsync(id, cancellationToken);
            return !result.IsSuccess
                ? BadRequest(result.Error?.Message)
                : NoContent();
        }

        // PATCH /api/admin/banners/{id}/toggle-status
        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(Guid id, CancellationToken cancellationToken)
        {
            var result = await _bannerService.ToggleBannerStatusAsync(id, cancellationToken);
            return !result.IsSuccess
                ? BadRequest(result.Error?.Message)
                : Ok();
        }
    }
}
