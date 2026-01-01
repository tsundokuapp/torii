using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TsundokuTraducoes.Auth.Api.AppServices.Interfaces;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.Entities;

namespace TsundokuTraducoes.Auth.Api.Controllers;


[ApiController]
public class LoginController : ControllerBase
{
    private readonly ILoginAppService _loginAppService;
    private readonly UserManager<CustomIdentityUser> _userManager;
    

    public LoginController(ILoginAppService loginAppService)
    {
        _loginAppService = loginAppService;
    }

    [HttpPost("api/auth/login/")]
    public async Task<IActionResult> LogarUsuario(LoginRequest loginRequest)
    {
        if (loginRequest.UserName == null)
            return BadRequest(new { message = "Nome de usuário é obrigatório." });

        if (loginRequest.Password == null)
            return BadRequest(new { message = "Senha é obrigatória." });
        
        var result = await _loginAppService.LogaUsuario(loginRequest);

        if (result.IsFailed)
            return Unauthorized(result.Errors[0].Message);

        var login = result.Value;
        
        Response.Cookies.Append(
            "refresh_token",
            login.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax, // SameSiteMode.Strict, usar esse se tiver no mesmo domínio.
                Expires  = login.RefreshTokenExpiry,
                Path = "api/auth/refresh-token"
            }
        );

        return Ok( new
        {
            login.UserName,
            login.AccessToken,
        });
    }

    [HttpGet("api/auth/recuperar-senha")]
    public async Task<IActionResult> RecuperarSenha(string email)
    {
        var result = await _loginAppService.RecuperarSenha(email);
        if (result.IsFailed)
            return Unauthorized(result.Errors[0]);

        return Ok(result.Successes[0]);
    }

    [HttpPost("api/auth/resetar-senha")]
    public async Task<IActionResult> ResetarSenha(ResetarSenhaRequest resetarSenhaRequest)
    {
        var result = await _loginAppService.ResetarSenha(resetarSenhaRequest);
        if (result.IsFailed)
            return Unauthorized(result.Errors[0]);

        return Ok(result.Successes[0]);
    }
}