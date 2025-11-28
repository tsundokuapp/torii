using FluentResults;
using TsundokuTraducoes.Auth.Api.DTOs.Request;

namespace TsundokuTraducoes.Auth.Api.Services.Interfaces;

public interface ICadastroService
{
    Task<Result<object>> CadastrarUsuario(CadastroUsuarioRequest cadastroUsuarioDTO);
    Task<Result> AtivaContaUsuario(AtivaUsuarioRequest ativaUsuarioRequest);
}
