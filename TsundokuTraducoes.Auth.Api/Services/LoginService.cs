using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;
using System.Web;
using TsundokuTraducoes.Auth.Api.DTOs;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.DTOs.Response;
using TsundokuTraducoes.Auth.Api.Entities;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;
using TsundokuTraducoes.Helpers.Configuration;
using TsundokuTraducoes.Helpers.Utils.Enums;

namespace TsundokuTraducoes.Auth.Api.Services;

public class LoginService : ILoginService
{
    private readonly ITokenService _tokenService;
    public readonly IEmailService _emailServices;
    private readonly SignInManager<CustomIdentityUser> _signInManager;
    private readonly UserManager<CustomIdentityUser> _userManager;

    public LoginService(ITokenService tokenService, IEmailService emailServices, SignInManager<CustomIdentityUser> signInManager, UserManager<CustomIdentityUser> userManager)
    {
        _tokenService = tokenService;
        _emailServices = emailServices;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<Result<LoginInternalResult>> LogaUsuario(LoginRequest loginRequest)
    {
        var usuario = _userManager.Users.FirstOrDefault(x => x.NormalizedUserName == loginRequest.UserName.ToUpper());

        if (usuario == null)
            return Result.Fail("Erro ao logar: usuário ou senha inválidos");
        
        if (!await _signInManager.CanSignInAsync(usuario))
        {
            // Se a autenticação por celular for habilitada, esse trecho precisa ser refatorado.
            return Result.Fail("Não é possível logar sem confirmar o e-mail.");
        }
        
        var resultadoIdentity = await _signInManager.PasswordSignInAsync(
            loginRequest.UserName,
            loginRequest.Password,
            false, 
            false
        );
        
        if (!resultadoIdentity.Succeeded)
            return Result.Fail("Erro ao logar: usuário ou senha inválidos");

        
        var refreshToken = _tokenService.GenerateRefreshToken();
        
        usuario.RefreshToken = refreshToken;
        usuario.RefreshTokenExpiryTime = 
            DateTime.Now.AddMinutes(
                ConfigurationAutenticacaoExternal.RetornaRefreshTokenValidityInMinutes()
            );

        await _userManager.UpdateAsync(usuario);

        //Criando e retornando token com roles
        var token = _tokenService.CreateToken(usuario, _signInManager.UserManager.GetRolesAsync(usuario).Result.ToList());

        return Result.Ok(new LoginInternalResult
        {
            UserName = usuario.UserName,
            AccessToken = token.Value,
            RefreshToken = usuario.RefreshToken,
            RefreshTokenExpiry = usuario.RefreshTokenExpiryTime
        });
    }

    public async Task<Result> RecuperarSenha(string email)
    {
        var identityUser = _signInManager.UserManager.Users.FirstOrDefault(x => x.NormalizedEmail == email.ToUpper());
        if (identityUser != null)
        {
            var codigoRedefinicaoSenha = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(identityUser);
            var token = _tokenService.GenerateResetToken(identityUser.Id);

            var tokenRecuperacaoSenha = new TokenRecuperacaoSenha
            {
                CodigoRedefinicaoSenha = codigoRedefinicaoSenha,
                TokenResetSenha = token
            };

            var bytesTokenRecuperacaoSenha = ObjectToByteArray(tokenRecuperacaoSenha);

            var encodedToken = WebEncoders.Base64UrlEncode(bytesTokenRecuperacaoSenha);

            var codigoConfirmacaoEncododo = HttpUtility.UrlEncode(encodedToken);

            var linkReset = $"{ConfigurationAutenticacaoExternal.RetornaUrlBaseWeb()}/resetar-senha?Token={codigoConfirmacaoEncododo}";

            _emailServices.EnviaEmail([email], "Recuperar a senha", linkReset, TipoEnvioEmailEnum.RecuperacaoSenha);

            return Result.Ok().WithSuccess(codigoConfirmacaoEncododo);
        }

        return Result.Fail("Falha ao tentar redefinir a senha");
    }

    public async Task<Result> ResetarSenha(ResetarSenhaRequest resetarSenhaRequest)
    {
        var tokenRecuracaoSenha = _tokenService.ValidateTokenAndGetUserId(resetarSenhaRequest.Token);

        if (tokenRecuracaoSenha == null)
            return Result.Fail("Token inválido ou expirado.");
             
        var identityUser = _signInManager.UserManager.Users.FirstOrDefault(x => x.Id == tokenRecuracaoSenha.Id);
        var resultadoIdentity = await _signInManager.UserManager.ResetPasswordAsync(identityUser, tokenRecuracaoSenha.CodigoRedefinicaoSenha, resetarSenhaRequest.NovaSenha);
        if (resultadoIdentity.Succeeded)
        {
            return Result.Ok().WithSuccess("Senha alterada com sucesso!");
        }

        return Result.Fail("Falha na alteração de senha");
    }

    public static byte[] ObjectToByteArray<T>(T obj)
    {
        string json = JsonSerializer.Serialize(obj);
        return Encoding.UTF8.GetBytes(json);
    }
}