using FluentResults;
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
        if (loginRequest.UserName == null)
        {
            return BadRequest(new { message = "Nome de usuário é obrigatório." });
        }

        if (loginRequest.Password == null)
        {
            return BadRequest(new { message = "Senha é obrigatória." });
        }
        
        var result = await _loginAppService.LogaUsuario(loginRequest);

        if (result.IsFailed)
            return Unauthorized(result.Errors[0]);

        return Ok(result.ValueOrDefault);
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