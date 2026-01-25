using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;

namespace TsundokuTraducoes.Auth.Api.Controllers;

/// <summary>
/// Controller para gerenciamento de permissões dinâmicas de usuários.
/// Permissões usam formato hierárquico: recurso.acao[.contexto.id]
/// </summary>
[ApiController]
[Route("api/permissoes-usuario")]
[Authorize(Roles = "admin")]
public class PermissoesUsuarioController : ControllerBase
{
    private readonly IClaimsService _claimsService;

    public PermissoesUsuarioController(IClaimsService claimsService)
    {
        _claimsService = claimsService;
    }

    /// <summary>
    /// Lista todas as permissões dinâmicas de um usuário.
    /// Não inclui permissões herdadas das roles.
    /// </summary>
    [HttpGet("usuario/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPermissoesUsuario(int userId)
    {
        var result = await _claimsService.ObterPermissoesPorUsuario(userId);
        if (result.IsFailed)
            return NotFound(new { error = result.Errors[0].Message });

        return Ok(result.Value);
    }

    /// <summary>
    /// Atribui uma nova permissão específica a um usuário.
    /// Formato: recurso.acao[.contexto.id]
    /// Exemplos:
    /// - capitulo.deletar.obra.minha-novel.5
    /// - obra.editar.minha-novel
    /// - capitulo.*.obra.minha-novel (wildcard)
    /// </summary>
    [HttpPost("usuario/{userId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AtribuirPermissao(int userId, [FromBody] PermissaoUsuarioRequest request)
    {
        var atribuirRequest = new AtribuirPermissaoRequest
        {
            UserId = userId,
            Permissao = request.Permissao,
            Descricao = request.Descricao,
            ExpiraEm = request.ExpiraEm
        };

        var result = await _claimsService.AtribuirPermissao(atribuirRequest);
        if (result.IsFailed)
            return BadRequest(new { error = result.Errors[0].Message });

        return Created($"api/permissoes-usuario/{result.Value.Id}", result.Value);
    }

    /// <summary>
    /// Atualiza uma permissão existente.
    /// </summary>
    [HttpPut("{permissaoId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarPermissao(int permissaoId, [FromBody] PermissaoUsuarioRequest request)
    {
        var result = await _claimsService.AtualizarPermissao(permissaoId, request);
        if (result.IsFailed)
            return BadRequest(new { error = result.Errors[0].Message });

        return Ok(result.Value);
    }

    /// <summary>
    /// Remove uma permissão específica do usuário.
    /// </summary>
    [HttpDelete("{permissaoId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoverPermissao(int permissaoId)
    {
        var result = await _claimsService.RemoverPermissao(permissaoId);
        if (result.IsFailed)
            return NotFound(new { error = result.Errors[0].Message });

        return NoContent();
    }

    /// <summary>
    /// Verifica se um usuário tem uma permissão específica.
    /// Considera permissões das roles + permissões dinâmicas.
    /// Suporta wildcards.
    /// </summary>
    [HttpGet("usuario/{userId}/verificar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerificarPermissao(int userId, [FromQuery] string permissao)
    {
        if (string.IsNullOrEmpty(permissao))
            return BadRequest(new { error = "Permissão é obrigatória" });

        var result = await _claimsService.TemPermissao(userId, permissao);
        if (result.IsFailed)
            return NotFound(new { error = result.Errors[0].Message });

        return Ok(new { temPermissao = result.Value, permissao });
    }
}
