using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/banner")]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        // GET /api/admin/banner
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var banners = await _bannerService.GetAllBannersAsync(cancellationToken);
            return !banners.IsSuccess
                ? BadRequest(banners.Error?.Message)
                : Ok(banners);
        }

        // POST /api/admin/banner
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateAndUpdateBannerDto dto, CancellationToken cancellationToken)
        {
            var banner = await _bannerService.CreateBannerAsync(dto, cancellationToken);
            return banner.IsSuccess ? Ok(banner.Value) : BadRequest(banner.Error);
        }

        // PUT /api/admin/banner/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] CreateAndUpdateBannerDto dto, CancellationToken cancellationToken)
        {
            var banner = await _bannerService.UpdateBannerAsync(id, dto, cancellationToken);
            return banner.IsSuccess ? Ok(banner.Value) : NotFound(banner.Error);
        }

        // DELETE /api/admin/banner/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _bannerService.DeleteBannerAsync(id, cancellationToken);
            return !result.IsSuccess
                ? BadRequest(result.Error?.Message)
                : NoContent();
        }

        // PUT /api/admin/banner/{id}
        [HttpPut("status/{id}")]
        public async Task<IActionResult> ToggleStatus(Guid id, CancellationToken cancellationToken)
        {
            var result = await _bannerService.ToggleBannerStatusAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(new { success = true }) : NotFound(result.Error);
        }
    }
}
