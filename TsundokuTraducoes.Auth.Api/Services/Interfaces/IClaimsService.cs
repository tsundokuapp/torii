using FluentResults;
using TsundokuTraducoes.Auth.Api.DTOs.Request;
using TsundokuTraducoes.Auth.Api.DTOs.Response;

namespace TsundokuTraducoes.Auth.Api.Services.Interfaces;

/// <summary>
/// Serviço para gerenciamento de permissões dinâmicas de usuários.
/// Permissões são no formato hierárquico: recurso.acao[.contexto.id]
/// </summary>
public interface IClaimsService
{
    /// <summary>
    /// Obtém todas as permissões dinâmicas de um usuário.
    /// </summary>
    Task<Result<IEnumerable<PermissaoResponse>>> ObterPermissoesPorUsuario(int userId);

    /// <summary>
    /// Atribui uma permissão específica a um usuário.
    /// </summary>
    Task<Result<PermissaoResponse>> AtribuirPermissao(AtribuirPermissaoRequest request);

    /// <summary>
    /// Atualiza uma permissão existente.
    /// </summary>
    Task<Result<PermissaoResponse>> AtualizarPermissao(int permissaoId, PermissaoUsuarioRequest request);

    /// <summary>
    /// Remove uma permissão específica do usuário.
    /// </summary>
    Task<Result> RemoverPermissao(int permissaoId);

    /// <summary>
    /// Verifica se usuário tem uma permissão específica (inclui verificação de wildcards).
    /// </summary>
    Task<Result<bool>> TemPermissao(int userId, string permissaoRequerida);
}
