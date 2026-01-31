using FluentResults;
using Microsoft.AspNetCore.Mvc;
using TsundokuTraducoes.Auth.Api.AppServices.Interfaces;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.Entities;

namespace TsundokuTraducoes.Auth.Api.Controllers;

[ApiController]
public class CadastroController : ControllerBase
{
    private readonly ICadastroAppService _usuarioAppService;

    public CadastroController(ICadastroAppService usuarioAppService)
    {
        _usuarioAppService = usuarioAppService;
    }

    [HttpPost("api/auth/cadastro/")]
    public async Task<IActionResult> CadastraUsuario([FromBody] CadastroUsuarioRequest cadastroUsuarioRequest)
    {
        
        if (cadastroUsuarioRequest.Usuario == null)
        {
            return BadRequest(new { message = "Nome de usuário é obrigatório." });
        }
        
        if (cadastroUsuarioRequest.Senha == null)
        {
            return BadRequest(new { message = "Senha é obrigatório." });
        }
        B
        if (cadastroUsuarioRequest.Email == null)
        {
            return BadRequest(new { message = "E-mail é obrigatório." });
        }
        
        var result = await _usuarioAppService.CadastrarUsuario(cadastroUsuarioRequest);
        if (result.IsFailed)
            return BadRequest(result.Errors[0].Message);

        return Ok(result);
    }

    [HttpGet("api/auth/ativar-conta/")]
    public async Task<IActionResult> AtivaContaUsuario([FromQuery] AtivaUsuarioRequest ativaUsuarioRequest)
    {
        var result = await _usuarioAppService.AtivaContaUsuario(ativaUsuarioRequest);
        if (result.IsFailed)
            return BadRequest(result.Errors[0].Message);

        return Ok(result.Successes[0]);
    }
}
