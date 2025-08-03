using FluentResults;
using TsundokuTraducoes.Auth.Api.AppServices.Interfaces;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;

namespace TsundokuTraducoes.Auth.Api.AppServices;

public class CadastroAppService : ICadastroAppService
{
    private readonly ICadastroService _cadastroService;

    public CadastroAppService(ICadastroService usuarioService)
    {
        _cadastroService = usuarioService;
    }    

    public async Task<Result<object>> CadastrarUsuario(CadastroUsuarioRequest cadastroUsuarioRequest)
    {
        return await _cadastroService.CadastrarUsuario(cadastroUsuarioRequest);
    }

    public async Task<Result> AtivaContaUsuario(AtivaUsuarioRequest ativaUsuarioRequest)
    {
        return await _cadastroService.AtivaContaUsuario(ativaUsuarioRequest);
    }
}
