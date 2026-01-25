using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TsundokuTraducoes.Auth.Api.Data.Context;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.DTOs.Response;
using TsundokuTraducoes.Auth.Api.Entities;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;

namespace TsundokuTraducoes.Auth.Api.Services;

/// <summary>
/// Serviço para gerenciamento de permissões dinâmicas de usuários.
/// Permissões são no formato hierárquico: recurso.acao[.contexto.id]
/// </summary>
public class ClaimsService : IClaimsService
{
    private readonly UsuarioContext _context;
    private readonly UserManager<CustomIdentityUser> _userManager;
    private readonly IPermissionService _permissionService;

    public ClaimsService(
        UsuarioContext context,
        UserManager<CustomIdentityUser> userManager,
        IPermissionService permissionService)
    {
        _context = context;
        _userManager = userManager;
        _permissionService = permissionService;
    }

    public async Task<Result<IEnumerable<PermissaoResponse>>> ObterPermissoesPorUsuario(int userId)
    {
        var usuario = await _userManager.FindByIdAsync(userId.ToString());
        if (usuario == null)
            return Result.Fail<IEnumerable<PermissaoResponse>>("Usuário não encontrado.");

        var permissoes = await _context.ClaimsDinamicas
            .Where(c => c.UserId == userId && c.Ativo)
            .Where(c => c.ExpiraEm == null || c.ExpiraEm > DateTime.UtcNow)
            .Select(c => new PermissaoResponse
            {
                Id = c.Id,
                Valor = c.Valor,
                Descricao = c.Descricao ?? string.Empty,
                CriadoEm = c.CriadoEm,
                ExpiraEm = c.ExpiraEm,
                Ativo = c.Ativo
            })
            .ToListAsync();

        return Result.Ok<IEnumerable<PermissaoResponse>>(permissoes);
    }

    public async Task<Result<PermissaoResponse>> AtribuirPermissao(AtribuirPermissaoRequest request)
    {
        var usuario = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (usuario == null)
            return Result.Fail<PermissaoResponse>("Usuário não encontrado.");

        // Valida formato da permissão
        if (!ValidarFormatoPermissao(request.Permissao))
            return Result.Fail<PermissaoResponse>("Formato de permissão inválido. Use: recurso.acao[.contexto.id]");

        // Verifica se já existe permissão igual ATIVA e NÃO EXPIRADA
        var permissaoExistente = await _context.ClaimsDinamicas
            .FirstOrDefaultAsync(c =>
                c.UserId == request.UserId &&
                c.Valor == request.Permissao.ToLowerInvariant() &&
                c.Ativo &&
                (c.ExpiraEm == null || c.ExpiraEm > DateTime.UtcNow));

        if (permissaoExistente != null)
            return Result.Fail<PermissaoResponse>("Permissão já existe para este usuário.");

        var novaPermissao = new ClaimDinamica
        {
            UserId = request.UserId,
            Tipo = "Permission",
            Valor = request.Permissao.ToLowerInvariant(),
            Descricao = request.Descricao,
            ExpiraEm = request.ExpiraEm,
            CriadoEm = DateTime.UtcNow,
            Ativo = true
        };

        _context.ClaimsDinamicas.Add(novaPermissao);
        await _context.SaveChangesAsync();

        return Result.Ok(new PermissaoResponse
        {
            Id = novaPermissao.Id,
            Valor = novaPermissao.Valor,
            Descricao = novaPermissao.Descricao ?? string.Empty,
            CriadoEm = novaPermissao.CriadoEm,
            ExpiraEm = novaPermissao.ExpiraEm,
            Ativo = novaPermissao.Ativo
        });
    }

    public async Task<Result<PermissaoResponse>> AtualizarPermissao(int permissaoId, PermissaoUsuarioRequest request)
    {
        var permissao = await _context.ClaimsDinamicas.FindAsync(permissaoId);
        if (permissao == null)
            return Result.Fail<PermissaoResponse>("Permissão não encontrada.");

        // Valida formato da permissão
        if (!ValidarFormatoPermissao(request.Permissao))
            return Result.Fail<PermissaoResponse>("Formato de permissão inválido. Use: recurso.acao[.contexto.id]");

        permissao.Valor = request.Permissao.ToLowerInvariant();
        permissao.Descricao = request.Descricao;
        permissao.ExpiraEm = request.ExpiraEm;

        await _context.SaveChangesAsync();

        return Result.Ok(new PermissaoResponse
        {
            Id = permissao.Id,
            Valor = permissao.Valor,
            Descricao = permissao.Descricao ?? string.Empty,
            CriadoEm = permissao.CriadoEm,
            ExpiraEm = permissao.ExpiraEm,
            Ativo = permissao.Ativo
        });
    }

    public async Task<Result> RemoverPermissao(int permissaoId)
    {
        var permissao = await _context.ClaimsDinamicas.FindAsync(permissaoId);
        if (permissao == null)
            return Result.Fail("Permissão não encontrada.");

        _context.ClaimsDinamicas.Remove(permissao);
        await _context.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result<bool>> TemPermissao(int userId, string permissaoRequerida)
    {
        var usuario = await _userManager.FindByIdAsync(userId.ToString());
        if (usuario == null)
            return Result.Fail<bool>("Usuário não encontrado.");

        // Obtém roles do usuário
        var roles = await _userManager.GetRolesAsync(usuario);

        // Admin tem todas as permissões
        if (roles.Contains("admin"))
            return Result.Ok(true);

        // Obtém permissões das roles
        var permissoesRoles = await _context.RolePermissoes
            .Where(rp => roles.Contains(rp.Role!.Name!))
            .Select(rp => rp.Permissao!.Valor)
            .ToListAsync();

        // Obtém permissões dinâmicas do usuário
        var permissoesDinamicas = await _context.ClaimsDinamicas
            .Where(c => c.UserId == userId && c.Ativo)
            .Where(c => c.ExpiraEm == null || c.ExpiraEm > DateTime.UtcNow)
            .Select(c => c.Valor)
            .ToListAsync();

        // Combina todas as permissões
        var todasPermissoes = permissoesRoles.Concat(permissoesDinamicas);

        // Verifica se alguma permissão corresponde
        var temPermissao = todasPermissoes.Any(p =>
            _permissionService.PermissaoCorresponde(p, permissaoRequerida));

        return Result.Ok(temPermissao);
    }

    /// <summary>
    /// Valida se o formato da permissão está correto.
    /// Deve ter pelo menos 2 partes: recurso.acao
    /// </summary>
    private static bool ValidarFormatoPermissao(string permissao)
    {
        if (string.IsNullOrWhiteSpace(permissao))
            return false;

        var partes = permissao.Split('.');

        // Mínimo: recurso.acao (2 partes)
        // Wildcard especial *.* é permitido
        if (partes.Length < 2)
            return false;

        // Não permite partes vazias
        return partes.All(p => !string.IsNullOrWhiteSpace(p));
    }
}
