using System.Security.Claims;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;

namespace TsundokuTraducoes.Auth.Api.Services;

/// <summary>
/// Serviço para verificação de permissões hierárquicas com suporte a wildcards.
///
/// Formato das permissões: recurso.acao[.contexto.identificador]
///
/// Exemplos:
/// - obra.visualizar               → Visualizar qualquer obra
/// - obra.deletar                  → Deletar qualquer obra
/// - capitulo.criar.obra.teste     → Criar capítulos na obra "teste"
/// - capitulo.deletar.obra.teste.5 → Deletar o capítulo 5 da obra "teste"
/// - capitulo.*                    → Todas as ações em capítulos
/// - capitulo.*.obra.teste         → Todas as ações em capítulos da obra "teste"
/// - *.*                           → Super admin (todas as permissões)
/// </summary>
public class PermissionService : IPermissionService
{
    private const string PermissionClaimType = "Permission";
    private const string Wildcard = "*";

    public bool TemPermissao(ClaimsPrincipal user, string permissaoRequerida)
    {
        if (user == null || string.IsNullOrEmpty(permissaoRequerida))
            return false;

        // Admin tem todas as permissões
        if (user.IsInRole("admin"))
            return true;

        var permissoesUsuario = ObterPermissoes(user);

        return permissoesUsuario.Any(p => PermissaoCorresponde(p, permissaoRequerida));
    }

    public bool PermissaoCorresponde(string permissaoUsuario, string permissaoRequerida)
    {
        if (string.IsNullOrEmpty(permissaoUsuario) || string.IsNullOrEmpty(permissaoRequerida))
            return false;

        // Normaliza para lowercase
        permissaoUsuario = permissaoUsuario.ToLowerInvariant();
        permissaoRequerida = permissaoRequerida.ToLowerInvariant();

        // Permissão exata
        if (permissaoUsuario == permissaoRequerida)
            return true;

        // Super wildcard (*.*)
        if (permissaoUsuario == "*.*" || permissaoUsuario == "*")
            return true;

        var partesUsuario = permissaoUsuario.Split('.');
        var partesRequerida = permissaoRequerida.Split('.');

        for (int i = 0; i < partesUsuario.Length; i++)
        {
            // Wildcard no final: aceita tudo após este ponto
            if (partesUsuario[i] == Wildcard)
            {
                // Se é o último segmento do padrão, aceita qualquer continuação
                if (i == partesUsuario.Length - 1)
                    return true;

                // Wildcard no meio: pula um segmento e continua comparando
                continue;
            }

            // Se a permissão requerida tem menos partes que o padrão
            if (i >= partesRequerida.Length)
                return false;

            // Comparação exata do segmento
            if (partesUsuario[i] != partesRequerida[i])
                return false;
        }

        // Verifica se todas as partes foram consumidas
        return partesUsuario.Length == partesRequerida.Length;
    }

    public IEnumerable<string> ObterPermissoes(ClaimsPrincipal user)
    {
        if (user == null)
            return Enumerable.Empty<string>();

        return user.Claims
            .Where(c => c.Type == PermissionClaimType)
            .Select(c => c.Value)
            .Distinct();
    }
}
