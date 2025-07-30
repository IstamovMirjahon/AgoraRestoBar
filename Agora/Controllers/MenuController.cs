using Agora.Application.DTOs;
using Agora.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _service;

        public MenuController(IMenuService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateUpdateMenuDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] CreateUpdateMenuDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return result.IsSuccess ? Ok(new { success = true }) : NotFound(result.Error);
        }
    }

}
