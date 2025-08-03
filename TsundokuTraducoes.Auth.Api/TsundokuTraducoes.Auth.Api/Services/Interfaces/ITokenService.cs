using FluentResults;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.DTOs.Response;
using TsundokuTraducoes.Auth.Api.Entities;

namespace TsundokuTraducoes.Auth.Api.Services.Interfaces;

public interface ITokenService
{
    Token CreateToken(CustomIdentityUser usuario, List<string> roles);

    Task<Result<TokenResponse>> RefreshToken(TokenRequest tokenRequest);
    string GenerateRefreshToken();
}
