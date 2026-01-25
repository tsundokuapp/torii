using Microsoft.AspNetCore.Authorization;
using TsundokuTraducoes.Auth.Api.Authorization.Requirements;
using TsundokuTraducoes.Auth.Api.Services.Interfaces;

namespace TsundokuTraducoes.Auth.Api.Authorization.Handlers;

/// <summary>
/// Handler para verificar permissões hierárquicas com suporte a wildcards.
/// Usa o PermissionService para verificação consistente.
/// </summary>
public class PermissaoHandler : AuthorizationHandler<PermissaoRequirement>
{
    private readonly IPermissionService _permissionService;

    public PermissaoHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissaoRequirement requirement)
    {
        var user = context.User;

        // Admin tem acesso total
        if (user.IsInRole("admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Usa o PermissionService para verificar (suporta wildcards)
        if (_permissionService.TemPermissao(user, requirement.Permissao))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
