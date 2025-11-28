using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Web;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.Entities;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;
using TsundokuTraducoes.Helpers.Configuration;
using TsundokuTraducoes.Helpers.Utils.Enums;

namespace TsundokuTraducoes.Auth.Api.Services;

public class CadastroService : ICadastroService
{
    public readonly IMapper _mapper;
    public readonly IEmailService _emailServices;
    public UserManager<CustomIdentityUser> _userManager;

    public CadastroService(IMapper mapper, IEmailService emailServices, UserManager<CustomIdentityUser> userManager)
    {
        _mapper = mapper;
        _emailServices = emailServices;
        _userManager = userManager;
    }

    public async Task<Result<object>> CadastrarUsuario(CadastroUsuarioRequest cadastroUsuarioDTO)
    {
        var usuarioExistente = _userManager.Users.FirstOrDefault(x => x.NormalizedEmail == cadastroUsuarioDTO.Email.ToUpper());
        if (usuarioExistente is not null)
            return Result.Fail("E-mail já cadastrado");

        var usuario = _mapper.Map<Usuario>(cadastroUsuarioDTO);
        CustomIdentityUser usuarioIdentity = _mapper.Map<CustomIdentityUser>(usuario);
        var resultadoIdentity = await _userManager.CreateAsync(usuarioIdentity, cadastroUsuarioDTO.Senha);

        if (!resultadoIdentity.Succeeded) 
            return Result.Fail(RetornaMensagemErroResultadoIdentity(resultadoIdentity));


        await _userManager.AddToRoleAsync(usuarioIdentity, "leitor");
        if (!resultadoIdentity.Succeeded)
            return Result.Fail(RetornaMensagemErroResultadoIdentity(resultadoIdentity));

        var codigoConfirmacao = _userManager.GenerateEmailConfirmationTokenAsync(usuarioIdentity).Result;
        var codigoConfirmacaoEncododo = HttpUtility.UrlEncode(codigoConfirmacao);

        var linkEnvioCodigoAtivacao = $"{ConfigurationAutenticacaoExternal.RetornaUrlBaseApi()}/ativar-conta?UsuarioId={usuarioIdentity.Id}&CodigoAtivacao={codigoConfirmacaoEncododo}";

        _emailServices.EnviaEmail([usuarioIdentity.Email], "Ativação de conta", linkEnvioCodigoAtivacao, TipoEnvioEmailEnum.AtivacaoConta);
        
        return Result.Ok(new { IdUsuario = usuarioIdentity.Id, CodigoConfirmacao = codigoConfirmacaoEncododo });
    }    

    public async Task<Result> AtivaContaUsuario(AtivaUsuarioRequest ativaUsuarioRequest)
    {
        var usuarioIdentity = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == ativaUsuarioRequest.UsuarioId);
        var resultadoAtivacao = await _userManager.ConfirmEmailAsync(usuarioIdentity, ativaUsuarioRequest.CodigoAtivacao);
        if (resultadoAtivacao.Succeeded)
        {
            return Result.Ok().WithSuccess("Ativação da conta realizada com sucesso!");
        }

        return Result.Fail("Falha na tentativa de ativação da conta!");
    }

    private static string RetornaMensagemErroResultadoIdentity(IdentityResult resultadoIdentity)
    {
        var mensagensErro = string.Empty;

        foreach (var erro in resultadoIdentity.Errors)
        {
            mensagensErro += erro.Description + Environment.NewLine;
        }

        return mensagensErro;
    }
}
