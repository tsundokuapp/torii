using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsundokuTraducoes.Auth.Api.Data.Context;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.DTOs.Response;
using TsundokuTraducoes.Auth.Api.Entities;

namespace TsundokuTraducoes.Auth.Api.Controllers;

/// <summary>
/// Controller para gerenciamento de permissões base do sistema.
/// Estas são as permissões que podem ser associadas a roles.
/// Formato hierárquico: recurso.acao[.contexto.id]
/// </summary>
[ApiController]
[Route("api/permissoes")]
[Authorize(Roles = "admin")]
public class PermissoesController : ControllerBase
{
    private readonly UsuarioContext _context;

    public PermissoesController(UsuarioContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista todas as permissões base do sistema.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarPermissoes()
    {
        var permissoes = await _context.Permissoes
            .Select(p => new PermissaoResponse
            {
                Id = p.Id,
                Valor = p.Valor,
                Descricao = p.Descricao,
                CriadoEm = p.CriadoEm,
                Ativo = true
            })
            .ToListAsync();
        return Ok(permissoes);
    }

    /// <summary>
    /// Cria uma nova permissão base no sistema.
    /// Formato: recurso.acao (ex: "capitulo.deletar")
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CriarPermissao([FromBody] PermissaoRequest request)
    {
        var valorNormalizado = request.Valor.ToLowerInvariant();

        var permissaoExistente = await _context.Permissoes
            .FirstOrDefaultAsync(p => p.Valor == valorNormalizado);

        if (permissaoExistente != null)
            return BadRequest(new { error = "Já existe uma permissão com este valor." });

        // Valida formato
        if (!ValidarFormatoPermissao(valorNormalizado))
            return BadRequest(new { error = "Formato inválido. Use: recurso.acao (ex: capitulo.deletar)" });

        var permissao = new Permissao
        {
            Valor = valorNormalizado,
            Descricao = request.Descricao,
            CriadoEm = DateTime.UtcNow
        };

        _context.Permissoes.Add(permissao);
        await _context.SaveChangesAsync();

        return Created($"api/permissoes/{permissao.Id}", new PermissaoResponse
        {
            Id = permissao.Id,
            Valor = permissao.Valor,
            Descricao = permissao.Descricao,
            CriadoEm = permissao.CriadoEm,
            Ativo = true
        });
    }

    /// <summary>
    /// Lista permissões associadas a uma role.
    /// </summary>
    [HttpGet("role/{roleName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarPermissoesPorRole(string roleName)
    {
        var permissoes = await _context.RolePermissoes
            .Where(rp => rp.Role!.Name == roleName)
            .Select(rp => new PermissaoResponse
            {
                Id = rp.Permissao!.Id,
                Valor = rp.Permissao.Valor,
                Descricao = rp.Permissao.Descricao,
                CriadoEm = rp.Permissao.CriadoEm,
                Ativo = true
            })
            .ToListAsync();

        return Ok(permissoes);
    }

    /// <summary>
    /// Associa uma permissão a uma role.
    /// </summary>
    [HttpPost("role/{roleName}/permissao/{permissaoId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssociarPermissaoRole(string roleName, int permissaoId)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        if (role == null)
            return NotFound(new { error = "Role não encontrada." });

        var permissao = await _context.Permissoes.FindAsync(permissaoId);
        if (permissao == null)
            return NotFound(new { error = "Permissão não encontrada." });

        var existe = await _context.RolePermissoes
            .AnyAsync(rp => rp.RoleId == role.Id && rp.PermissaoId == permissaoId);

        if (existe)
            return BadRequest(new { error = "Role já possui esta permissão." });

        _context.RolePermissoes.Add(new RolePermissao
        {
            RoleId = role.Id,
            PermissaoId = permissaoId,
            AtribuidoEm = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Permissão '{permissao.Valor}' associada à role '{roleName}'." });
    }

    /// <summary>
    /// Remove uma permissão de uma role.
    /// </summary>
    [HttpDelete("role/{roleName}/permissao/{permissaoId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoverPermissaoRole(string roleName, int permissaoId)
    {
        var rolePermissao = await _context.RolePermissoes
            .Include(rp => rp.Role)
            .FirstOrDefaultAsync(rp => rp.Role!.Name == roleName && rp.PermissaoId == permissaoId);

        if (rolePermissao == null)
            return NotFound(new { error = "Associação não encontrada." });

        _context.RolePermissoes.Remove(rolePermissao);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Remove uma permissão base do sistema.
    /// </summary>
    [HttpDelete("{permissaoId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoverPermissao(int permissaoId)
    {
        var permissao = await _context.Permissoes.FindAsync(permissaoId);
        if (permissao == null)
            return NotFound(new { error = "Permissão não encontrada." });

        _context.Permissoes.Remove(permissao);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static bool ValidarFormatoPermissao(string permissao)
    {
        if (string.IsNullOrWhiteSpace(permissao))
            return false;

        var partes = permissao.Split('.');
        return partes.Length >= 2 && partes.All(p => !string.IsNullOrWhiteSpace(p));
    }
}
