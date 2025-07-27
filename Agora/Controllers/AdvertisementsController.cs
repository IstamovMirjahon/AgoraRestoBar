using Agora.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Route("api/advertisements")]
    [ApiController]
    public class AdvertisementsController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        public AdvertisementsController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveBanners(CancellationToken cancellationToken)
        {
            var banners = await _bannerService.GetAllBannersAsync(cancellationToken);
            if (!banners.IsSuccess)
            {
                return BadRequest(banners.Error?.Message);
            }
            var activeBanners = banners.Value?.Where(b => b.IsActive);

            if (activeBanners == null || !activeBanners.Any())
            {
                return NotFound("No active advertisements found.");
            }
            return Ok(activeBanners);
        }
    }
}
