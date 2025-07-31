using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.Entities;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;

namespace TsundokuTraducoes.Auth.Api.Services;

public class UsuarioService : IUsuarioService
{
    public readonly IMapper _mapper;
    public readonly IEmailMimeService _emailServices;
    public UserManager<CustomIdentityUser> _userManager;

    public UsuarioService(IMapper mapper, IEmailMimeService emailServices, UserManager<CustomIdentityUser> userManager)
    {
        _mapper = mapper;
        _emailServices = emailServices;
        _userManager = userManager;
    }

    public async Task<Result<string>> CadastrarUsuario(CadastroUsuarioRequest cadastroUsuarioDTO)
    {
        var usuario = _mapper.Map<Usuario>(cadastroUsuarioDTO);
        CustomIdentityUser usuarioIdentity = _mapper.Map<CustomIdentityUser>(usuario);
        var resultadoIdentity = await _userManager.CreateAsync(usuarioIdentity, cadastroUsuarioDTO.Senha);

        if (!resultadoIdentity.Succeeded)
        {
            var mensagensErro = string.Empty;

            foreach (var erro in resultadoIdentity.Errors)
            {
                mensagensErro += erro.Description + Environment.NewLine;
            }            

            return Result.Fail(mensagensErro);
        }
            

        await _userManager.AddToRoleAsync(usuarioIdentity, "leitor");
        if (!resultadoIdentity.Succeeded)
        {
            var mensagensErro = string.Empty;

            foreach (var erro in resultadoIdentity.Errors)
            {
                mensagensErro += erro.Description + Environment.NewLine;
            }

            return Result.Fail(mensagensErro);
        }

        var codigoConfirmacao = _userManager.GenerateEmailConfirmationTokenAsync(usuarioIdentity).Result;
        _emailServices.EnviaEmail([usuarioIdentity.Email], "Ativação de conta", usuarioIdentity.Id, codigoConfirmacao);
        return Result.Ok().WithSuccess(codigoConfirmacao);        
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
}
