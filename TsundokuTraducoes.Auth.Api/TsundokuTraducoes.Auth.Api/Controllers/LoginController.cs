using Microsoft.AspNetCore.Mvc;
using TsundokuTraducoes.Auth.Api.AppServices.Interfaces;
using TsundokuTraducoes.Auth.Api.DTOs.Request;

namespace TsundokuTraducoes.Auth.Api.Controllers;

[ApiController]
public class LoginController : ControllerBase
{
    private readonly ILoginAppService _loginAppService;

    public LoginController(ILoginAppService loginAppService)
    {
        _loginAppService = loginAppService;
    }

    [HttpPost("api/auth/login/")]
    public async Task<IActionResult> LogarUsuario(LoginRequest loginRequest)
    {
        var result = await _loginAppService.LogaUsuario(loginRequest);

        if (result.IsFailed)
            return Unauthorized(result.Errors[0]);

        return Ok(result.ValueOrDefault);
    }
}