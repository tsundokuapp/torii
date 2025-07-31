using FluentResults;
using TsundokuTraducoes.Auth.Api.DTOs.Request;

namespace TsundokuTraducoes.Auth.Api.Services.Interfaces;

public interface IUsuarioService
{
    Task<Result<string>> CadastrarUsuario(CadastroUsuarioRequest cadastroUsuarioDTO);
    Task<Result> AtivaContaUsuario(AtivaUsuarioRequest ativaUsuarioRequest);
}
