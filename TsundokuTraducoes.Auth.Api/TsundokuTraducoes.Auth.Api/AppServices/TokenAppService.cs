using FluentResults;
using TsundokuTraducoes.Auth.Api.AppServices.Interfaces;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.DTOs.Response;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;

namespace TsundokuTraducoes.Auth.Api.AppServices;

public class TokenAppService : ITokenAppService
{
    private readonly ITokenService _tokenService;

    public TokenAppService(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<Result<TokenResponse>> RefreshToken(TokenRequest tokenRequest)
    {
        return await _tokenService.RefreshToken(tokenRequest);
    }
}
