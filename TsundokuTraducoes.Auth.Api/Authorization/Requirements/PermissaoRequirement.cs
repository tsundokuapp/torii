using Microsoft.AspNetCore.Authorization;

namespace TsundokuTraducoes.Auth.Api.Authorization.Requirements;

/// <summary>
/// Requirement para verificar permissões hierárquicas.
/// Formato: recurso.acao[.contexto.id]
///
/// Exemplos:
/// - new PermissaoRequirement("capitulo.deletar")
/// - new PermissaoRequirement("capitulo.deletar.obra.minha-novel.5")
/// </summary>
public class PermissaoRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Permissão hierárquica requerida
    /// </summary>
    public string Permissao { get; }

    public PermissaoRequirement(string permissao)
    {
        Permissao = permissao.ToLowerInvariant();
    }

    /// <summary>
    /// Cria um requirement a partir de componentes individuais.
    /// </summary>
    public static PermissaoRequirement Create(string recurso, string acao, params string[] contexto)
    {
        var partes = new List<string> { recurso, acao };
        partes.AddRange(contexto);
        return new PermissaoRequirement(string.Join(".", partes));
    }
}
