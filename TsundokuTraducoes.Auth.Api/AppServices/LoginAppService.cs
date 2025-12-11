using FluentResults;
using TsundokuTraducoes.Auth.Api.AppServices.Interfaces;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.DTOs.Response;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;

namespace TsundokuTraducoes.Auth.Api.AppServices;

public class LoginAppService : ILoginAppService
{
    private readonly ILoginService _loginService;

    public LoginAppService(ILoginService loginService)
    {
        _loginService = loginService;
    }

    public async Task<Result<LoginResponse>> LogaUsuario(LoginRequest loginRequest)
    {
        return await _loginService.LogaUsuario(loginRequest);
    }

    public async Task<Result> RecuperarSenha(string email)
    {
        return await _loginService.RecuperarSenha(email);
    }

    public async Task<Result> ResetarSenha(ResetarSenhaRequest resetarSenhaRequest)
    {
        return await _loginService.ResetarSenha(resetarSenhaRequest);
    }
}