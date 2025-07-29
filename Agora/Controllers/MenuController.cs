using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/admin/menus")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        // GET /api/admin/menus?lang=uz
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string lang = "uz")
        {
            var result = await _menuService.GetAllAsync(lang);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error?.Message);
        }

        // GET /api/admin/menus/{id}?lang=uz
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] string lang = "uz")
        {
            var result = await _menuService.GetByIdAsync(id, lang);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error?.Message);
        }

        // POST /api/admin/menus
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateandUpdateMenuDto dto)
        {
            var result = await _menuService.CreateAsync(dto);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error?.Message);
        }

        // PUT /api/admin/menus/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] CreateandUpdateMenuDto dto)
        {
            var result = await _menuService.UpdateAsync(id, dto);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error?.Message);
        }

        // DELETE /api/admin/menus/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _menuService.DeleteAsync(id);
            return result.IsSuccess ? NoContent() : NotFound(result.Error?.Message);
        }
    }
}
