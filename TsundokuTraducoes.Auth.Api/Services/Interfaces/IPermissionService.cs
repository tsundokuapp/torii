using System.Security.Claims;

namespace TsundokuTraducoes.Auth.Api.Services.Interfaces;

/// <summary>
/// Serviço para verificação de permissões hierárquicas com suporte a wildcards.
/// Formato: recurso.acao[.contexto.id]
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Verifica se o usuário tem a permissão requerida.
    /// Suporta wildcards (*) nas permissões do usuário.
    /// </summary>
    /// <param name="user">ClaimsPrincipal do usuário autenticado</param>
    /// <param name="permissaoRequerida">Permissão específica requerida (ex: "capitulo.deletar.obra.teste.5")</param>
    /// <returns>True se tem permissão</returns>
    bool TemPermissao(ClaimsPrincipal user, string permissaoRequerida);

    /// <summary>
    /// Verifica se uma permissão do usuário corresponde à permissão requerida.
    /// </summary>
    /// <param name="permissaoUsuario">Permissão que o usuário possui (pode ter wildcards)</param>
    /// <param name="permissaoRequerida">Permissão específica requerida</param>
    /// <returns>True se corresponde</returns>
    bool PermissaoCorresponde(string permissaoUsuario, string permissaoRequerida);

    /// <summary>
    /// Obtém todas as permissões do usuário (roles + claims dinâmicas).
    /// </summary>
    IEnumerable<string> ObterPermissoes(ClaimsPrincipal user);
}
