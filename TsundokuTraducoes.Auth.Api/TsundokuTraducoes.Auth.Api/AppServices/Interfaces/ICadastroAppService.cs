using FluentResults;
using TsundokuTraducoes.Auth.Api.DTOs.Request;

namespace TsundokuTraducoes.Auth.Api.AppServices.Interfaces;

public interface ICadastroAppService
{
    Task<Result<object>> CadastrarUsuario(CadastroUsuarioRequest cadastroUsuarioDTO);

    Task<Result> AtivaContaUsuario(AtivaUsuarioRequest ativaUsuarioRequest);
}
