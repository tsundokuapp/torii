using Microsoft.AspNetCore.Mvc;
using TsundokuTraducoes.Auth.Api.AppServices.Interfaces;
using TsundokuTraducoes.Auth.Api.DTOs.Request;

namespace TsundokuTraducoes.Auth.Api.Controllers;

[ApiController]
public class CadastroController : ControllerBase
{
    private readonly IUsuarioAppService _usuarioAppService;

    public CadastroController(IUsuarioAppService usuarioAppService)
    {
        _usuarioAppService = usuarioAppService;
    }

    [HttpPost("api/auth/cadastro/")]
    public async Task<IActionResult> CadastraUsuario([FromBody] CadastroUsuarioRequest cadastroUsuarioRequest)
    {
        var result = await _usuarioAppService.CadastrarUsuario(cadastroUsuarioRequest);
        if (result.IsFailed)
            return BadRequest(result.Errors[0].Message);

        return Ok(result.Value);
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
