using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;

namespace TsundokuTraducoes.Auth.Api.Authorization.Attributes;

/// <summary>
/// Atributo de autorização baseado em permissões hierárquicas.
/// Formato: recurso.acao[.contexto.id]
/// Admin sempre tem acesso.
///
/// Exemplos de uso:
/// [RequirePermission("obra.criar")]
/// [RequirePermission("capitulo.deletar")]
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class RequirePermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string _permissao;

    /// <summary>
    /// Requer uma permissão hierárquica.
    /// </summary>
    /// <param name="permissao">Permissão no formato recurso.acao (ex: "capitulo.deletar")</param>
    public RequirePermissionAttribute(string permissao)
    {
        _permissao = permissao.ToLowerInvariant();
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Admin pode tudo
        if (user.IsInRole("admin"))
            return;

        // Obtém o PermissionService do container de DI
        var permissionService = context.HttpContext.RequestServices
            .GetService<IPermissionService>();

        if (permissionService == null)
        {
            context.Result = new StatusCodeResult(500);
            return;
        }

        // Verifica se tem a permissão (suporta wildcards)
        var possuiPermissao = permissionService.TemPermissao(user, _permissao);

        if (!possuiPermissao)
        {
            context.Result = new ForbidResult();
        }
    }
}

/// <summary>
/// Atributo legado para compatibilidade. Use RequirePermissionAttribute.
/// </summary>
[Obsolete("Use RequirePermissionAttribute com formato hierárquico")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class PermissaoAuthorizeAttribute : RequirePermissionAttribute
{
    public PermissaoAuthorizeAttribute(string recurso, string acao)
        : base($"{recurso}.{acao}")
    {
    }
}
