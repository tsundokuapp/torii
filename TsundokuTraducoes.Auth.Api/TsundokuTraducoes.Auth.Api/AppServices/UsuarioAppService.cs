using FluentResults;
using TsundokuTraducoes.Auth.Api.AppServices.Interfaces;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;

namespace TsundokuTraducoes.Auth.Api.AppServices;

public class UsuarioAppService : IUsuarioAppService
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioAppService(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }    

    public async Task<Result<string>> CadastrarUsuario(CadastroUsuarioRequest cadastroUsuarioRequest)
    {
        return await _usuarioService.CadastrarUsuario(cadastroUsuarioRequest);
    }

    public async Task<Result> AtivaContaUsuario(AtivaUsuarioRequest ativaUsuarioRequest)
    {
        return await _usuarioService.AtivaContaUsuario(ativaUsuarioRequest);
    }
}
