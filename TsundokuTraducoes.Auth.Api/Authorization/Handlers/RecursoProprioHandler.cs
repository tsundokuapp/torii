using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TsundokuTraducoes.Auth.Api.Authorization.Requirements;

namespace TsundokuTraducoes.Auth.Api.Authorization.Handlers;

/// <summary>
/// Handler para verificar se o usuário é dono do recurso
/// Admin pode acessar qualquer recurso
/// </summary>
public class RecursoProprioHandler : AuthorizationHandler<RecursoProprioRequirement, IRecursoComProprietario>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RecursoProprioRequirement requirement,
        IRecursoComProprietario recurso)
    {
        var userIdClaim = context.User.FindFirst("id")?.Value;

        // Admin pode acessar qualquer recurso
        if (context.User.IsInRole("admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Usuário só pode acessar recursos próprios
        if (!string.IsNullOrEmpty(userIdClaim) &&
            int.TryParse(userIdClaim, out int userId) &&
            recurso.ProprietarioId == userId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
