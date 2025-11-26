using Microsoft.AspNetCore.Mvc;
using TsundokuTraducoes.Auth.Api.AppServices.Interfaces;
using TsundokuTraducoes.Auth.Api.DTOs.Request;

namespace TsundokuTraducoes.Auth.Api.Controllers;

[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenAppService _tokenAppService;

    public TokenController(ITokenAppService tokenAppService)
    {
        _tokenAppService = tokenAppService;
    }
        
    [HttpPost("api/auth/refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenRequest tokenRequest)
    {
        if (tokenRequest is null)
            return BadRequest("Token request inválido");

        var result = await _tokenAppService.RefreshToken(tokenRequest);

        if (result.IsFailed)
            return BadRequest(result.Errors[0]);

        return Ok(result.ValueOrDefault);
    }
}