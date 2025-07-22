using Agora.Application.DTOs;
using Agora.Application.DTOs.Errors;
using Agora.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Route.Api.Controllers.Admin;

[Route("api/admin/auth")]
[ApiController]
public class AuthAdminController(
    IAuthService authService,
    ResponseSerializer responseSerializer) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    //[SuccessResponse(typeof(LoginResponse), "Success")]
    //[ProducesResponseType(typeof(UnauthorizedError), 401)]
    //[ProducesApiError(CommonFieldErrorCodes.InvalidValue, LoginErrorMessages.UsernameOrPasswordInvalid)]
    //[ProducesApiError(CommonFieldErrorCodes.ValueDoesNotExist, LoginErrorMessages.UsernameOrPasswordWrong)]
    public async Task<IActionResult> Login([FromBody] AdminLoginDto request)
    {
        if (request == null)
        {
            return BadRequest("Login ma'lumotlari to'g'ri emas");
        }

        var result = await authService.LoginAsync(request);
        return responseSerializer.ToActionResult(result);
    }

    [HttpDelete("logout")]
    [Authorize] // Faqat login qilingan foydalanuvchilar chiqishi mumkin
    //[SuccessResponse(typeof(bool), "Success")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

        if(userId == null)
        {
            return BadRequest("Foydalanuvchi ID topilmadi");
        }
        var result = await authService.LogoutAsync(Guid.Parse(userId));
        return responseSerializer.ToActionResult(result);
    }

    [HttpGet("refresh-token")]
    [AllowAnonymous]
    //[SuccessResponse(typeof(LoginResponse), "Success")]
    public async Task<IActionResult> RefreshToken([FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return BadRequest("Refresh token bo‘sh bo‘lishi mumkin emas");
        }

        var result = await authService.RefreshTokenAsync(token);
        return responseSerializer.ToActionResult(result);
    }
}
