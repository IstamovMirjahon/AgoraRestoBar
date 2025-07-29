using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("admin/menu")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string lang = "uz")
        {
            var result = await _menuService.GetAllAsync(lang);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error?.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] string lang = "uz")
        {
            var result = await _menuService.GetByIdAsync(id, lang);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error?.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateandUpdateMenuDto dto)
        {
            var result = await _menuService.CreateAsync(dto);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error?.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] CreateandUpdateMenuDto dto)
        {
            var result = await _menuService.UpdateAsync(id, dto);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error?.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _menuService.DeleteAsync(id);
            return result.IsSuccess ? NoContent() : NotFound(result.Error?.Message);
        }
    }

}
