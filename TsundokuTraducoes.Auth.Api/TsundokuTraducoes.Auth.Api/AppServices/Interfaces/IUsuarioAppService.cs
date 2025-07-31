using FluentResults;
using TsundokuTraducoes.Auth.Api.DTOs.Request;

namespace TsundokuTraducoes.Auth.Api.AppServices.Interfaces;

public interface IUsuarioAppService
{
    Task<Result<string>> CadastrarUsuario(CadastroUsuarioRequest cadastroUsuarioDTO);

    Task<Result> AtivaContaUsuario(AtivaUsuarioRequest ativaUsuarioRequest);
}
