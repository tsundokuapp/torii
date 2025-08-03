using FluentResults;
using Microsoft.AspNetCore.Identity;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.DTOs.Response;
using TsundokuTraducoes.Auth.Api.Entities;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;
using TsundokuTraducoes.Helpers.Configuration;

namespace TsundokuTraducoes.Auth.Api.Services;

public class LoginService : ILoginService
{
    private readonly ITokenService _tokenService;
    private readonly SignInManager<CustomIdentityUser> _signInManager;
    private readonly UserManager<CustomIdentityUser> _userManager;

    public LoginService(ITokenService tokenService, SignInManager<CustomIdentityUser> signInManager, UserManager<CustomIdentityUser> userManager)
    {
        _tokenService = tokenService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<Result<TokenResponse>> LogaUsuario(LoginRequest loginRequest)
    {
        var resultadoIdentity = await _signInManager.PasswordSignInAsync(loginRequest.UserName, loginRequest.Password, false, false);

        if (resultadoIdentity.Succeeded)
        {
            var usuario = _userManager.Users.FirstOrDefault(x => x.NormalizedUserName == loginRequest.UserName.ToUpper());

            var refreshToken = _tokenService.GenerateRefreshToken();
            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(ConfigurationAutenticacaoExternal.RetornaRefreshTokenValidityInMinutes());

            await _userManager.UpdateAsync(usuario);

            //Criando e retornando token com roles
            var token = _tokenService.CreateToken(usuario, _signInManager.UserManager.GetRolesAsync(usuario).Result.ToList());

            return Result.Ok(new TokenResponse
            {
                AccessToken = token.Value,
                RefreshToken = usuario.RefreshToken
            });
        }

        return Result.Fail("Erro na tentativa de login");
    }
}
