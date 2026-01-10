using Microsoft.AspNetCore.Mvc;
using TsundokuTraducoes.Auth.Api.AppServices.Interfaces;

namespace TsundokuTraducoes.Auth.Api.Controllers;

[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenAppService _tokenAppService;

    public TokenController(ITokenAppService tokenAppService)
    {
        _tokenAppService = tokenAppService;
    }
        
    [HttpGet("api/auth/refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["tsun_refresh_token"];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized();

        var result = await _tokenAppService.RefreshToken(refreshToken);

        if (result.IsFailed)
            return BadRequest(result.Errors[0]);
        
        Response.Cookies.Append(
            "tsun_refresh_token",
            result.Value.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax, // SameSiteMode.Strict, usar esse se tiver no mesmo domínio.
                Expires  = result.Value.RefreshTokenExpiry,
                Path = "/"
            }
        );

        return Ok( new
        {
            result.Value.UserName,
            result.Value.AccessToken,
            result.Value.TsunId
        });
    }
}