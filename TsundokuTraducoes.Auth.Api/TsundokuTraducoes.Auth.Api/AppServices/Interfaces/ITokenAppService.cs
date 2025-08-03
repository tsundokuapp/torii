using FluentResults;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.DTOs.Response;

namespace TsundokuTraducoes.Auth.Api.AppServices.Interfaces;

public interface ITokenAppService
{
    Task<Result<TokenResponse>> RefreshToken(TokenRequest tokenRequest);
}
