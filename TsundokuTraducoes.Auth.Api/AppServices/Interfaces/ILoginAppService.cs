using FluentResults;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.DTOs.Response;

namespace TsundokuTraducoes.Auth.Api.AppServices.Interfaces;

public interface ILoginAppService
{
    Task<Result<LoginInternalResult>> LogaUsuario(LoginRequest loginRequest);
    Task<Result> RecuperarSenha(string email);
    Task<Result> ResetarSenha(ResetarSenhaRequest resetarSenhaRequest);
}
